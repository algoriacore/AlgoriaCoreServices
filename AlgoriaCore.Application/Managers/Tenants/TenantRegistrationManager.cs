using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.Emails.Groups;
using AlgoriaCore.Application.Managers.Emails.Groups.Dto;
using AlgoriaCore.Application.Managers.Emails.Templates;
using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.Managers.Roles.Dto;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Application.Managers.Tenants.Dto;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Domain.Email;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Interfaces.Exceptions;
using AlgoriaCore.Domain.MultiTenancy;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Tenants
{
    public class TenantRegistrationManager : BaseManager
    {
        private readonly IRepository<TenantRegistration, int> _repTenant;
        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly MailGroupManager _mailGroupManager;
        private readonly MailTemplateManager _mailTemplateManager;
        private readonly SettingManager _settingManager;
        private readonly LanguageManager _languageManager;
        private readonly IAppAuthorizationProvider _appAuthorizationProvider;
        private readonly IExceptionService _exceptionService;

        public TenantRegistrationManager(IRepository<TenantRegistration, int> repTenant,
                                        TenantManager tenantManager,
                                        UserManager userManager,
                                        RoleManager roleManager,
                                        MailGroupManager mailGroupManager,
                                        MailTemplateManager mailTemplateManager,
                                        SettingManager settingManager,
                                        LanguageManager languageManager,
                                        IAppAuthorizationProvider appAuthorizationProvider,
                                        IExceptionService exceptionService
                                        )
        {
            _repTenant = repTenant;
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailGroupManager = mailGroupManager;
            _mailTemplateManager = mailTemplateManager;
            _settingManager = settingManager;
            _languageManager = languageManager;
            _appAuthorizationProvider = appAuthorizationProvider;
            _exceptionService = exceptionService;
        }

        public async Task<string> CreateTenantRegistrationAsync(TenantRegistrationDto dto, bool sendEmail = true)
        {
			TenantDto tReg = null;

			using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.SoftDelete))
			{
				tReg = await _tenantManager.GetTenantByTenancyNameAsync(dto.TenancyName);
			}

            if (tReg != null)
            {
                _exceptionService.ThrowTenantRegistrationDuplicatedTenancyName(dto.TenancyName);
            }

            // Validar la complejidad de la contraseña
            await _userManager.ValidatePasswordComplexityAsync(dto.Password);

			var mEntity = await _repTenant.FirstOrDefaultAsync(m => m.TenancyName == dto.TenancyName);

			// Si no existe un "pre-registro" con este tenant, entonces se inserta el registro en la tabla "tenantRegistration.
			// Si el pre-registro ya existe, entonces sólo se actualiza
			// Se hace el ajuste en atención a la incidencia "00000020"
			if (mEntity != null)
			{
				_repTenant.Delete(mEntity);
				CurrentUnitOfWork.SaveChanges();
			}

			var entity = new TenantRegistration();
			entity.TenancyName = dto.TenancyName;
			entity.TenantName = dto.TenantName;
            entity.UserLogin = "admin"; // Este es el usuario default siempre que se crea un tenant
            entity.Password = dto.Password;
            entity.Name = dto.Name;
            entity.Lastname = dto.LastName;
            entity.SecondLastname = dto.SecondLastName;
            entity.EmailAddress = dto.EmailAddress;
            entity.ConfirmationCode = Guid.NewGuid().ToString();

			_repTenant.Insert(entity);
			CurrentUnitOfWork.SaveChanges();

            if (sendEmail)
            {
                // enviar correo de nuevo registro
                await SendEmailRegistration(entity);
            }

            return entity.ConfirmationCode;
        }

        public async Task<int> ConfirmTenantRegistrationAsync(string confirmationCode)
        {
            var entity = await _repTenant.FirstOrDefaultAsync(m => m.ConfirmationCode == confirmationCode);

            if (entity == null)
            {
                throw new EntityNotFoundException(L("Register.Tenant.ConfirmationCodeNotFound"));
            }

            var tenantId = await CreateTenantAndUserAsync(entity);

            // Eliminar el registro de "Tenant registration"
            _repTenant.Delete(entity);
            CurrentUnitOfWork.SaveChanges();

            return tenantId;
        }

        private async Task<int> CreateTenantAndUserAsync(TenantRegistration entity)
        {
            int tenantId = 0;

            var l = new TenantDto();
            l.TenancyName = entity.TenancyName;
            l.Name = entity.TenantName;
            l.CreationTime = Clock.Now;
            l.IsActive = true;
            l.IsDeleted = false;

            tenantId = await _tenantManager.CreateTenantAsync(l);

			if (tenantId > 0)
			{
				using (CurrentUnitOfWork.SetTenantId(tenantId))
				{
					UserDto uDto = new UserDto();
					uDto.TenantId = tenantId;
					uDto.Login = entity.UserLogin;

					PasswordHasher<UserDto> p = new PasswordHasher<UserDto>();
					uDto.Password = p.HashPassword(uDto, entity.Password);

					uDto.Name = entity.Name;
					uDto.LastName = entity.Lastname;
					uDto.SecondLastName = entity.SecondLastname;
					uDto.EmailAddress = entity.EmailAddress;
					uDto.IsEmailConfirmed = true;
					uDto.CreationTime = Clock.Now;
					uDto.ChangePassword = false;
					uDto.UserLocked = false;
					uDto.IsActive = true;
					uDto.IsDeleted = false;

					var uId = await _userManager.CreateUserAsync(uDto, null);

					await CreateSettingsForTenant(tenantId, uId);
				}
			}

            return tenantId;
        }

		private async Task CreateSettingsForTenant(int tenantId, long userId)
		{
			// Leer el archivo de configuración de "correos y configuraciones" para el tenant
			string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
			basePath = basePath.Replace("file:\\", "").Replace("file:", "");

			string fileName = "createtenantconfig.json";
			string filePath = Path.Combine(basePath, @"Managers\Tenants\config", fileName);

			if (!System.IO.File.Exists(filePath))
			{
				throw new AlgoriaCoreGeneralException(L("Register.Tenant.SettingsFileNotFound"));
			}

			var str = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
			var cnf = JsonConvert.DeserializeObject<CreateTenantConfigDto>(str);

			if (cnf == null)
			{
				throw new AlgoriaCoreGeneralException(L("Register.Tenant.NoSettingsFound"));
			}

			if (cnf.Settings == null || cnf.Settings.Count <= 0)
			{
				throw new AlgoriaCoreGeneralException(L("Register.Tenant.NoSettingsFound"));
			}

			if (cnf.Settings != null)
			{
				foreach (var s in cnf.Settings)
				{
					_settingManager.ChangeSetting(s.SettingKey, s.Value);
				}
			}

			await CreateEmailSettingsForTenant(cnf.Emails);
			await CreateLanguageSettingsForTenant(cnf.Languages);
			await CreateRolSettingsForTenant(cnf.Roles, tenantId, userId);
		}

		private async Task CreateEmailSettingsForTenant(List<MailTemplateDto> emails)
		{
			// Crear las plantillas de correo "default" para el tenant recién generado
			MailGroupDto mailDto = new MailGroupDto();
			mailDto.DisplayName = "Default";
			mailDto.IsSelected = true;

			var milGrpId = await _mailGroupManager.CreateMailGroupAsync(mailDto);

			_settingManager.ChangeSetting(AppSettings.General.MailGroup, milGrpId.ToString());

			if (emails != null && emails.Count > 0)
			{
				foreach (var e in emails)
				{
					e.MailGroup = milGrpId;
					await _mailTemplateManager.CreateMailTemplateAsync(e);
				}
			}
		}

		private async Task CreateLanguageSettingsForTenant(List<Languages.Dto.LanguageDto> languages)
		{
			if (languages != null && languages.Count > 0)
			{
				foreach (var lang in languages)
				{
					await _languageManager.CreateLanguageAsync(lang);
				}

				// Establecer el primer dto de la lista como lenguaje default
				var lst = await _languageManager.GetLanguageActiveAsync();
				var langDef = lst.FirstOrDefault(m => m.Name == languages[0].Name);
				if (langDef != null)
				{
					_languageManager.SetLanguageDefault(langDef.Id ?? 0);
				}
			}
		}

		private async Task CreateRolSettingsForTenant(List<RoleDto> roles, int tenantId, long userId)
		{
			if (roles != null)
			{
                var permisoList = _appAuthorizationProvider.GetPermissionNamesList(MultiTenancySides.Tenant) ?? new List<string>();
                var pNames = _appAuthorizationProvider.GetPermissionsFromNamesByValidating(permisoList);

				foreach (var r in roles)
				{
					RoleDto rol = new RoleDto();
					rol.Name = "admin";
					rol.DisplayName = "Administrador";
					rol.IsActive = true;
					rol.TenantId = tenantId;

					var rolId = await _roleManager.AddRoleAsync(rol, pNames.Select(m => m.DisplayName).ToList());
					rol.Id = rolId;

					var permissions = permisoList.Select(m => new PermissionDto
					{
						Name = m,
						RoleId = rolId,
						IsGranted = true
					}).ToList();

					await _roleManager.ReplacePermissionAsync(rolId, permissions);

					// Asignar los roles al usuario recién generado
					List<RoleDto> rList = new List<RoleDto>();
					rList.Add(rol);
					await _userManager.ReplaceRolesAsync(userId, rList);
				}
			}
		}

		private async Task SendEmailRegistration(TenantRegistration entity)
        {
            MailTemplateDto emailTemplateDto = null;
            string rootAddress = string.Empty;

            using (var uow = CurrentUnitOfWork.SetTenantId(null))
            {
                emailTemplateDto = await _mailTemplateManager.GetMailTemplateCurrentByMailKey(EmailKeys.TenantRegistration);

                var emp = await _settingManager.GetSettingValueAsync(AppSettings.General.WebSiteRootAddress);

                if (emp != null)
                {
                    rootAddress = emp;
                    if (!rootAddress.EndsWith("/"))
                    {
                        rootAddress += "/";
                    }
                }
            }

            string fullName = string.Format("{0}{1}{2}", !entity.Name.IsNullOrEmpty() ? entity.Name + " ": "", 
                !entity.Lastname.IsNullOrEmpty() ? entity.Lastname + " " : string.Empty, 
                !entity.SecondLastname.IsNullOrEmpty() ? entity.SecondLastname : string.Empty);

            var emailAddress = new EmailAddress { Address = entity.EmailAddress, Name = fullName };

            var dict = new Dictionary<string, string>();
            dict.Add(EmailVariables.TenantRegistration.TenantShortName.ToParam(), entity.TenancyName);
            dict.Add(EmailVariables.TenantRegistration.TenantName.ToParam(), entity.TenantName);
            dict.Add(EmailVariables.TenantRegistration.Name.ToParam(), entity.Name);
            dict.Add(EmailVariables.TenantRegistration.LastName.ToParam(), entity.Lastname);
            dict.Add(EmailVariables.TenantRegistration.SecondLastName.ToParam(), entity.SecondLastname);
            dict.Add(EmailVariables.TenantRegistration.Email.ToParam(), entity.EmailAddress);
            dict.Add(EmailVariables.TenantRegistration.ConfirmationCode.ToParam(), entity.ConfirmationCode);
            dict.Add(EmailVariables.TenantRegistration.ActivationUrl.ToParam(), string.Format("{0}account/confirm?code={1}", rootAddress, entity.ConfirmationCode));

            SendEmail(emailTemplateDto, emailAddress, dict);
        }
    }
}
