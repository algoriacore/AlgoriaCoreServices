using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.Emails.Templates;
using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using AlgoriaCore.Application.Managers.Roles.Dto;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Application.Managers.Settings.Dto;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Domain.Email;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Users
{
    public class UserManager : BaseManager
	{
		private readonly IRepository<User, long> _repUser;
		private readonly IRepository<UserReset, long> _repUserReset;
		private readonly IRepository<Tenant, int> _repTenant;
		private readonly IRepository<UserRole, long> _repUserRole;
		private readonly IRepository<UserPasswordHistory, long> _repUserPasswordHistory;
		private readonly IRepository<Role, long> _repRole;

		private readonly MailTemplateManager _mailTemplateManager;
		private readonly SettingManager _settingManager;

		public UserManager(IRepository<User, long> repUser,
							IRepository<UserReset, long> repUserReset,
							IRepository<Tenant, int> repTenant,
							IRepository<UserRole, long> repUserRole,
							IRepository<UserPasswordHistory, long> repUserPasswordHistory,
							IRepository<Role, long> repRole,
							MailTemplateManager mailTemplateManager,
							SettingManager settingManager)
		{
			_repUser = repUser;
			_repUserReset = repUserReset;
			_repTenant = repTenant;
			_repUserRole = repUserRole;
			_repUserPasswordHistory = repUserPasswordHistory;
			_repRole = repRole;
			_mailTemplateManager = mailTemplateManager;
			_settingManager = settingManager;
		}

		public async Task<UserDto> GetUserByUserLoginAndTenancyNameAsync(string userName, string tenancyName)
		{
			UserDto userDto = null;

			bool isNullOrEmpty = tenancyName.IsNullOrEmpty();

			var a = GetUserQuery()
				.Where(m => (m.Login == userName || m.EmailAddress == userName) && (isNullOrEmpty || m.TenancyName == tenancyName));

			userDto = await a.FirstOrDefaultAsync();

			return userDto;
		}

		public async Task<UserDto> GetUserByUserName(string userName)
		{
			var query = GetUserQuery();
			var uDto = query.FirstOrDefault(m => m.Login == userName);
			if (uDto == null)
			{
				return null;
			}

			return await GetUserById(uDto.Id);
		}

		public async Task<PagedResultDto<UserDto>> GetUsersAsync(PageListByDto dto)
		{
			var a = GetUserQuery()
				.WhereIf(!dto.Filter.IsNullOrEmpty(),
						m => m.Id.ToString().Contains(dto.Filter.ToLower()) ||
						m.Login.ToLower().Contains(dto.Filter.ToLower()) ||
						m.Name.ToLower().Contains(dto.Filter.ToLower()) ||
						m.LastName.ToLower().Contains(dto.Filter.ToLower()) ||
						m.SecondLastName.ToLower().Contains(dto.Filter.ToLower()) ||
						((m.Name + " " + m.LastName).Trim() + " " + m.SecondLastName).Trim().ToLower().Contains(dto.Filter.ToLower()) ||
						m.EmailAddress.ToLower().Contains(dto.Filter.ToLower()) ||
						m.IsActiveDesc.ToLower().Contains(dto.Filter.ToLower())
					);

			var tot = await a.CountAsync();
			var lst = await a.OrderBy(dto.Sorting)
							 .PageByIf(dto.IsPaged, dto)
							 .ToListAsync();

			return new PagedResultDto<UserDto>(tot, lst);
		}

		public async Task<long> CreateUserAsync(UserDto dto, List<RolDto> rolesList)
		{
			var entity = new User();

			entity.TenantId = dto.TenantId ?? (SessionContext.TenantId ?? CurrentUnitOfWork.GetTenantId());
			entity.UserLogin = dto.Login;
			entity.Password = dto.Password;
			entity.Name = dto.Name;
			entity.Lastname = dto.LastName;
			entity.SecondLastname = dto.SecondLastName;
			entity.EmailAddress = dto.EmailAddress;
			entity.IsEmailConfirmed = dto.IsEmailConfirmed;
			entity.PhoneNumber = dto.PhoneNumber;
			entity.IsPhoneConfirmed = dto.IsPhoneNumberConfirmed;
			entity.CreationTime = DateTime.Now;
			entity.ChangePassword = dto.ChangePassword;
			entity.AccessFailedCount = dto.AccesFailedCount;
			entity.ProfilePictureId = dto.ProfilePictureId;
			entity.UserLocked = dto.UserLocked;
			entity.IsLockoutEnabled = dto.IsLockoutEnabled;
			entity.IsActive = dto.IsActive;
			entity.IsDeleted = dto.IsDeleted;

			_repUser.Insert(entity);
			CurrentUnitOfWork.SaveChanges();

			await LogChange(await GetUserById(entity.Id), null, rolesList, ChangeLogType.Create);

			return entity.Id;
		}

		public async Task<long> UpdateUserAsync(UserDto dto, List<RolDto> rolesList)
		{
			var entity = await _repUser.FirstOrDefaultAsync(m => m.Id == dto.Id);

			if (entity == null)
			{
				throw new EntityNotFoundException(L("Users.NotFound"));
			}

			var previousDto = await GetUserById(dto.Id);

			entity.UserLogin = dto.Login;
			entity.Password = dto.Password;
			entity.Name = dto.Name;
			entity.Lastname = dto.LastName;
			entity.SecondLastname = dto.SecondLastName;
			entity.EmailAddress = dto.EmailAddress;
			entity.IsEmailConfirmed = dto.IsEmailConfirmed;
			entity.PhoneNumber = dto.PhoneNumber;
			entity.IsPhoneConfirmed = dto.IsPhoneNumberConfirmed;
			entity.ChangePassword = dto.ChangePassword;
			entity.AccessFailedCount = dto.AccesFailedCount;
			entity.ProfilePictureId = dto.ProfilePictureId;
			entity.UserLocked = dto.UserLocked;
			entity.IsLockoutEnabled = dto.IsLockoutEnabled;
			entity.IsActive = dto.IsActive;
			entity.IsDeleted = dto.IsDeleted;

			CurrentUnitOfWork.SaveChanges();

			await LogChange(await GetUserById(entity.Id), previousDto, rolesList, ChangeLogType.Update);

			return entity.Id;
		}

		public async Task<UserDto> GetUserById(long id, bool throwExceptionIfNotFound = true)
		{
			UserDto dto = null;
			var entity = await _repUser.GetAllIncluding(p => p.Tenant).FirstOrDefaultAsync(m => m.Id == id);

			if (throwExceptionIfNotFound && entity == null)
			{
				throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Users.User"), id));
			}

			if (entity != null)
			{
				dto = GetUser(entity);
			}

			return dto;
		}

		public async Task<UserDto> GetUserOfSessionAsync()
		{
			return await GetUserById(SessionContext.UserId ?? 0);
		}

		public async Task<long> UpdateUserProfileAsync(UserDto dto)
		{
			var actualDto = await GetUserById(SessionContext.UserId ?? 0);

			actualDto.Name = dto.Name;
			actualDto.LastName = dto.LastName;
			actualDto.SecondLastName = dto.SecondLastName;
			actualDto.EmailAddress = dto.EmailAddress;
			actualDto.PhoneNumber = dto.PhoneNumber;

			if (dto.ProfilePictureId.HasValue)
			{
				actualDto.ProfilePictureId = dto.ProfilePictureId;
			}

			await UpdateUserAsync(actualDto, null);

			// revisar el cambio de contraseña
			if (!dto.Password.IsNullOrEmpty())
			{
				await ChangePasswordAsync(actualDto.Id, dto.Password);
			}

			return actualDto.Id;
		}

		public async Task<UserDto> GetUserByEmailAsync(string email)
		{
			var uDto = _repUser.FirstOrDefault(m => m.EmailAddress == email);
			if (uDto == null)
			{
				return null;
			}

			return await GetUserById(uDto.Id);
		}

		public async Task<UserDto> GetUserOrNullAsync(int? tenantId, long userId)
		{
			using (CurrentUnitOfWork.SetTenantId(tenantId))
			{
				return await GetUserById(userId);
			}
		}

		public async Task<UserDto> GetUserAsync(int? tenantId, long userId)
		{
			var user = await GetUserOrNullAsync(tenantId, userId);
			if (user == null)
			{
				throw new AlgoriaCoreGeneralException("There is no user: " + tenantId + "@" + userId);
			}

			return user;
		}

		public async Task<long> DeleteUserAsync(long id)
		{
			var dto = await GetUserById(id);

			if (dto == null)
			{
				throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Users"), id));
			}

			await _repUser.DeleteAsync(id);

			await CurrentUnitOfWork.SaveChangesAsync();

			await LogChange(null, dto, null, ChangeLogType.Delete);

			return id;
		}

		public async Task LockUserAsync(long id)
		{
			var dto = await GetUserById(id);

			if (dto == null)
			{
				throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Users"), id));
			}

			var entity = await _repUser.GetAsync(id);

			entity.UserLocked = true;

			await CurrentUnitOfWork.SaveChangesAsync();

			await LogChange(await GetUserById(id), dto, null, ChangeLogType.Update);
		}

		public async Task UnlockUserAsync(long id, bool sendEmail = true)
        {
			var dto = await GetUserById(id);

			if (dto == null)
			{
				throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Users"), id));
			}

			var entity = await _repUser.GetAsync(id);

			entity.UserLocked = false;
			entity.AccessFailedCount = 0;

			await CurrentUnitOfWork.SaveChangesAsync();

			var dtoUpdated = await GetUserById(id);

			await LogChange(dtoUpdated, dto, null, ChangeLogType.Update);

            if (sendEmail)
            {
                // Se envía correo al usuario
                await SendUnblockEmailAsync(dtoUpdated);
            }
        }

		#region Validar la complejidad de la contraseña

		public async Task ValidatePasswordComplexityAsync(string password)
		{
			PasswordComplexityDto pComplexity = await _settingManager.GetPasswordComplexitySettings();

			// Si hay una configuración de complejidad de contraseña
			if (pComplexity != null)
			{
				ValidatePasswordLengthAsync(pComplexity, password);
				ValidatePasswordLiteralsAsync(pComplexity, password);
			}
		}

		private void ValidatePasswordLengthAsync(PasswordComplexityDto pComplexity, string password)
		{
			if (password.Length < pComplexity.MinimumLength)
			{
				throw new AlgoriaCoreGeneralException(string.Format(L("Users.PasswordValidation.MinumumLength"), pComplexity.MinimumLength));
			}

			if (password.Length > pComplexity.MaximumLength)
			{
				throw new AlgoriaCoreGeneralException(string.Format(L("Users.PasswordValidation.MaximumLength"), pComplexity.MaximumLength));
			}
		}

		private void ValidatePasswordLiteralsAsync(PasswordComplexityDto pComplexity, string password)
		{
			if (pComplexity.UseNumbers && !Regex.IsMatch(password, "[0-9]{1}"))
			{
				throw new AlgoriaCoreGeneralException(L("Users.PasswordValidation.UseNumbers"));
			}

			if (pComplexity.UseUppercase && !Regex.IsMatch(password, "[A-Z]{1}"))
			{
				throw new AlgoriaCoreGeneralException(L("Users.PasswordValidation.UseUppercase"));
			}

			if (pComplexity.UseLowercase && !Regex.IsMatch(password, "[a-z]{1}"))
			{
				throw new AlgoriaCoreGeneralException(L("Users.PasswordValidation.UseLowercase"));
			}

			if (pComplexity.UsePunctuationSymbols && !Regex.IsMatch(password, "[^A-Za-z\\d]{1}"))
			{
				throw new AlgoriaCoreGeneralException(L("Users.PasswordValidation.UsePunctuationSymbols"));
			}
		}

		#endregion

		#region Métodos privados

		private UserDto GetUser(User entity)
		{
			var dto = new UserDto();

			dto.Id = entity.Id;
			dto.TenantId = entity.TenantId;
			dto.TenancyName = entity.TenantId.HasValue ? entity.Tenant.TenancyName : null;
			dto.Login = entity.UserLogin;
			dto.Password = entity.Password;
			dto.Name = entity.Name;
			dto.LastName = entity.Lastname;
			dto.SecondLastName = entity.SecondLastname;
			dto.EmailAddress = entity.EmailAddress;
			dto.IsEmailConfirmed = entity.IsEmailConfirmed;
			dto.PhoneNumber = entity.PhoneNumber;
			dto.IsPhoneNumberConfirmed = entity.IsPhoneConfirmed;
			dto.CreationTime = entity.CreationTime;
			dto.ChangePassword = entity.ChangePassword;
			dto.AccesFailedCount = entity.AccessFailedCount ?? 0;
			dto.ProfilePictureId = entity.ProfilePictureId;
			dto.UserLocked = entity.UserLocked;
			dto.IsLockoutEnabled = entity.IsLockoutEnabled;
			dto.IsActive = entity.IsActive;
			dto.IsDeleted = entity.IsDeleted;

			return dto;
		}

		private IQueryable<UserDto> GetUserQuery()
		{
			string yesLabel = L("Yes");
			string noLabel = L("No");

			var query = (from u in _repUser.GetAll()
						 join t in _repTenant.GetAll() on u.TenantId equals t.Id into tX
						 from t in tX.DefaultIfEmpty()
						 select new UserDto
						 {
							 Id = u.Id,
							 Login = u.UserLogin,
							 Password = u.Password,
							 Name = u.Name,
							 LastName = u.Lastname,
							 SecondLastName = u.SecondLastname,
							 FullName = ((u.Name + " " + u.Lastname).Trim() + " " + u.SecondLastname).Trim(),
							 EmailAddress = u.EmailAddress,
							 IsEmailConfirmed = u.IsEmailConfirmed,
							 PhoneNumber = u.PhoneNumber,
							 IsPhoneNumberConfirmed = u.IsPhoneConfirmed,
							 CreationTime = u.CreationTime,
							 ChangePassword = u.ChangePassword,
							 AccesFailedCount = u.AccessFailedCount ?? 0,
							 LastLoginTime = u.LastLoginTime,
							 ProfilePictureId = u.ProfilePictureId,
							 UserLocked = u.UserLocked,
							 UserLockedDesc = u.UserLocked == true ? yesLabel : noLabel,
							 IsLockoutEnabled = u.IsLockoutEnabled,
							 IsActive = u.IsActive,
							 IsActiveDesc = u.IsActive == true ? yesLabel : noLabel,
							 IsDeleted = u.IsDeleted,
							 TenancyName = t != null ? t.TenancyName : null,
							 TenantId = u.TenantId
						 });

			return query;
		}

		private async Task<long> LogChange(UserDto newDto, UserDto previousDto, List<RolDto> rolesList, ChangeLogType changeLogType)
		{
			if (newDto == null) { newDto = new UserDto(); }
			if (previousDto == null) { previousDto = new UserDto(); }

			StringBuilder sb = new StringBuilder("");

			if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
			{
				LogStringProperty(sb, previousDto.Login, newDto.Login, "{{Users.UserNameForm}}");
				LogStringProperty(sb, previousDto.Name, newDto.Name, "{{Users.NameForm}}");
				LogStringProperty(sb, previousDto.LastName, newDto.LastName, "{{Users.LastNameForm}}");
				LogStringProperty(sb, previousDto.SecondLastName, newDto.SecondLastName, "{{Users.SecondLastNameForm}}");
				LogStringProperty(sb, previousDto.EmailAddress, newDto.EmailAddress, "{{Users.EmailAddressForm}}");
				LogStringProperty(sb, previousDto.PhoneNumber, newDto.PhoneNumber, "{{Users.PhoneNumberForm}}");
				LogBoolProperty(sb, previousDto.ChangePassword ?? false, newDto.ChangePassword ?? false, "{{Users.ShouldChangePasswordOnNextLoginForm}}");
				LogBoolProperty(sb, previousDto.IsActive, newDto.IsActive, "{{Status}}");

				if (rolesList != null)
				{
					sb.AppendFormat("{0}: {1}", "{{Roles}}", string.Join(',', rolesList.Select(m => m.DisplayName)));
				}
			}

			return await LogChange(changeLogType, newDto.Id.ToString(), "User", sb.ToString());
		}

        #endregion

        #region Reset Password

        public async Task<string> CreateResetPasswordCodeAsync(string userLogin, string tenancyName)
        {
            var u = await GetUserByUserLoginAndTenancyNameAsync(userLogin, tenancyName);

            if (u == null)
            {
                throw new EntityNotFoundException(L("Users.NotFound"));
            }

            var uR = await _repUserReset.FirstOrDefaultAsync(m => m.UserId == u.Id);

            if (uR == null)
            {
                uR = new UserReset();
                uR.UserId = u.Id;

                _repUserReset.Insert(uR);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            uR.ResetCode = Guid.NewGuid().ToString();
            uR.Validity = Clock.Now.AddDays(1);

            await CurrentUnitOfWork.SaveChangesAsync();

            // Se envía correo al usuario solicitante
            await SendResetPasswordEmailAsync(u, uR.ResetCode);

            return uR.ResetCode;
        }

        public async Task<long> ChangePasswordFromResetCodeAsync(string confirmationCode, string newPassword)
		{
			var ll = await _repUserReset.FirstOrDefaultAsync(m => m.ResetCode == confirmationCode);

			if (ll == null)
			{
				throw new EntityNotFoundException(L("Users.Confimation.NotFound"));
			}

			if (Clock.Now > ll.Validity)
			{
				throw new AlgoriaCoreGeneralException(L("Users.Confimation.Expired"));
			}

			User uDto = null;

			using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
			{
				using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
				{
					uDto = await _repUser.FirstOrDefaultAsync(m => m.Id == ll.UserId);
				}
			}

			if (uDto == null)
			{
				throw new EntityNotFoundException(L("Users.NotFound"));
			}

			// Antes de guardar la nueva contraseña, se debe revisar si en la configuración está activado la "reutilización de contraseña"
			// Si está activado, entonces se validan los últimos X contraseñas guardadas
			await ValidateRepeatedPassword(uDto.TenantId, uDto.Id, newPassword);

			// Validar la complejidad de la contraseña
			using (var s = CurrentUnitOfWork.SetTenantId(uDto.TenantId))
			{
				await ValidatePasswordComplexityAsync(newPassword);
			}

			// Se guarda el password anterior
			string oldPassword = uDto.Password;

			PasswordHasher<UserDto> p = new PasswordHasher<UserDto>();
			uDto.Password = p.HashPassword(new UserDto(), newPassword);
			uDto.ChangePassword = false;
			CurrentUnitOfWork.SaveChanges();

			// Se elimina el registro de la tabla UserReset
			await _repUserReset.DeleteAsync(ll);
			await CurrentUnitOfWork.SaveChangesAsync();

			// Se guarda el historial de contraseñas del usuario
			await SavePasswordHistoryAsync(uDto.Id, oldPassword);

			return uDto.Id;
		}

		public async Task<long> ChangePasswordAsync(long userId, string newPassword)
		{
			User uDto = null;

			using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
			{
				using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
				{
					uDto = await _repUser.FirstOrDefaultAsync(m => m.Id == userId);
				}
			}

			if (uDto == null)
			{
				throw new EntityNotFoundException(L("Users.NotFound"));
			}

			// Antes de guardar la nueva contraseña, se debe revisar si en la configuración está activado la "reutilización de contraseña"
			// Si está activado, entonces se validan los últimos X contraseñas guardadas
			await ValidateRepeatedPassword(uDto.TenantId, uDto.Id, newPassword);

			// Se guarda el password anterior
			string oldPassword = uDto.Password;

			PasswordHasher<UserDto> p = new PasswordHasher<UserDto>();
			uDto.Password = p.HashPassword(new UserDto(), newPassword);
			uDto.ChangePassword = false;

			await CurrentUnitOfWork.SaveChangesAsync();

			// Se elimina el registro de la tabla UserReset
			var resetDto = await _repUserReset.FirstOrDefaultAsync(m => m.UserId == userId);

			if (resetDto != null)
			{
				_repUserReset.Delete(resetDto);
				await CurrentUnitOfWork.SaveChangesAsync();
			}

			// Se guarda el historial de contraseñas del usuario
			await SavePasswordHistoryAsync(uDto.Id, oldPassword);

			return uDto.Id;
		}

		private async Task ValidateRepeatedPassword(int? tenantId, long userId, string newPassword)
		{
			using (CurrentUnitOfWork.SetTenantId(tenantId))
			{
				var conf = await _settingManager.GetSettingValueAsync(AppSettings.Security.EnablePasswordReuseValidation);

				if (conf != null && (conf.ToLower() == "true" || conf.ToLower() == "1"))
				{
					var pHist = await GetPasswordHistoryList(userId);

					foreach (var h in pHist)
					{
						PasswordHasher<UserDto> pg = new PasswordHasher<UserDto>();

						if (pg.VerifyHashedPassword(new UserDto(), h.Password, newPassword) == PasswordVerificationResult.Success)
						{
							throw new AlgoriaCoreGeneralException(L("ChangePassword.PasswordHasAlreadyUsed"));
						}
					}
				}
			}
		}

		public async Task SavePasswordHistoryAsync(long userId, string password)
		{
			var x = new UserPasswordHistory();
			x.UserId = userId;
			x.Password = password;
			x.Date = Clock.Now;

			await _repUserPasswordHistory.InsertAsync(x);
			CurrentUnitOfWork.SaveChanges();
		}

		public async Task<List<UserPasswordHistoryDto>> GetPasswordHistoryList(long userId)
		{
			var ll = await _repUserPasswordHistory.GetAll()
					.Where(m => m.UserId == userId)
					.Select(m => new UserPasswordHistoryDto
					{
						Id = m.Id,
						UserId = m.UserId,
						Password = m.Password,
						Date = m.Date
					}).ToListAsync();

			return ll;
		}

		public async Task UpdateLoginInfo(long userId, int accesFailedCount)
		{
			var u = await _repUser.FirstOrDefaultAsync(m => m.Id == userId);

			if (u != null)
			{
				u.LastLoginTime = Clock.Now;
				u.AccessFailedCount = accesFailedCount;

				await CurrentUnitOfWork.SaveChangesAsync();
			}
		}

		#endregion

		#region Envío de correos

		public async Task SendResetPasswordEmailAsync(UserDto entity, string confirmationCode)
		{
			MailTemplateDto emailTemplateDto = null;
			string rootAddress = string.Empty;

			emailTemplateDto = await _mailTemplateManager.GetMailTemplateCurrentByMailKey(EmailKeys.UserReset);

			using (CurrentUnitOfWork.SetTenantId(null))
			{
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

			string fullName = string.Format("{0}{1}{2}", !entity.Name.IsNullOrEmpty() ? entity.Name + " " : "",
				!entity.LastName.IsNullOrEmpty() ? entity.LastName + " " : string.Empty,
				!entity.SecondLastName.IsNullOrEmpty() ? entity.SecondLastName : string.Empty);

			var emailAddress = new EmailAddress { Address = entity.EmailAddress, Name = fullName };

			var dict = new Dictionary<string, string>();
			dict.Add(EmailVariables.ResetPassword.UserName.ToParam(), entity.Login);
			dict.Add(EmailVariables.ResetPassword.Name.ToParam(), entity.Name);
			dict.Add(EmailVariables.ResetPassword.LastName.ToParam(), entity.LastName);
			dict.Add(EmailVariables.ResetPassword.SecondLastName.ToParam(), entity.SecondLastName);
			dict.Add(EmailVariables.ResetPassword.FullName.ToParam(), fullName);
			dict.Add(EmailVariables.ResetPassword.Email.ToParam(), entity.EmailAddress);
			dict.Add(EmailVariables.ResetPassword.ConfirmationCode.ToParam(), confirmationCode);
			dict.Add(EmailVariables.ResetPassword.ResetUrl.ToParam(), string.Format("{0}account/reset-password?code={1}", rootAddress, confirmationCode));

			SendEmail(emailTemplateDto, emailAddress, dict);
		}

		public void SendNewUserEmailAsync(UserDto userDest, string password, MailTemplateDto templateDto)
		{
			var emailAddress = new EmailAddress { Address = userDest.EmailAddress, Name = userDest.FullName };

			var dict = new Dictionary<string, string>();
			dict.Add(EmailVariables.NewUser.FullName.ToParam(), userDest.FullName);
			dict.Add(EmailVariables.NewUser.UserName.ToParam(), userDest.Login);
			dict.Add(EmailVariables.NewUser.Password.ToParam(), password);

			SendEmail(templateDto, emailAddress, dict);
		}

		public void SendChangePasswordEmailAsync(UserDto userDest, MailTemplateDto templateDto)
		{
			var emailAddress = new EmailAddress { Address = userDest.EmailAddress, Name = userDest.FullName };

			var dict = new Dictionary<string, string>() { { EmailVariables.ChangePassword.FullName.ToParam(), userDest.FullName } };

			SendEmail(templateDto, emailAddress, dict);
		}

		public async Task SendUnblockEmailAsync(UserDto userDest)
		{
			MailTemplateDto emailTemplateDto = await _mailTemplateManager.GetMailTemplateCurrentByMailKey(EmailKeys.UserUnblock);
			var emailAddress = new EmailAddress { Address = userDest.EmailAddress, Name = userDest.FullName };
			var dict = new Dictionary<string, string>() { { EmailVariables.ChangePassword.FullName.ToParam(), userDest.FullName } };

			SendEmail(emailTemplateDto, emailAddress, dict);
		}

		#endregion

		#region Roles

		public async Task<List<UserRoleDto>> GetRoleListByUser(long userId)
		{
			var ll = await (from us in _repUserRole.GetAll()
							join r in _repRole.GetAll() on us.RoleId equals r.Id
							where us.UserId == userId
							select new UserRoleDto
							{
								Id = us.Id,
								RoleId = r.Id,
								RoleName = r.Name,
								RoleDisplayName = r.DisplayName
							}).ToListAsync();

			return ll;
		}

		public async Task ReplaceRolesAsync(long userId, List<RolDto> rolList)
		{
			var ll = await _repUserRole.GetAll().Where(m => m.UserId == userId).ToListAsync();

			foreach (var r in rolList.Select(m => m.Id))
			{
				var rol = ll.FirstOrDefault(m => m.RoleId == r);

				if (rol != null)
				{
					ll.Remove(rol);
				}
				else
				{
					_repUserRole.Insert(new UserRole { UserId = userId, RoleId = r });
					await CurrentUnitOfWork.SaveChangesAsync();
				}
			}

			foreach (var entity in ll)
			{
				await _repUserRole.DeleteAsync(entity);
				await CurrentUnitOfWork.SaveChangesAsync();
			}
		}

		#endregion
	}
}
