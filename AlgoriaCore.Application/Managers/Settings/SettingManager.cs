using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Configuration;
using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using AlgoriaCore.Application.Managers.Settings.Dto;
using AlgoriaCore.Domain.Email;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaCore.Extensions.Utils;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Settings
{
    public class SettingManager : BaseManager
    {
        private readonly IRepository<Setting, long> _repository;
        private readonly IOptions<EmailOptions> _emailOptions;

        public SettingManager(IRepository<Setting, long> repository, IOptions<EmailOptions> emailOptions)
        {
            _repository = repository;
            _emailOptions = emailOptions;
        }

        private string GetSettingValue(string name)
		{
			return GetSettingValue(name, null);
		}

		private string GetSettingValue(string name, long? userId)
		{
			var query = GetSettingQuery();

			var queryCondition = query.Where(m => m.Name == name)
							.WhereIf(!userId.HasValue, m => m.UserId == null)
							.WhereIf(userId.HasValue, m => m.UserId == userId.Value);

			var sto = queryCondition.FirstOrDefault();

			if (sto != null)
			{
				return sto.Value;
			}
			else
			{
                //TODO: Desarrollar estrategia para buscar configuración default donde aplique
                return null;
			}
		}

		public T GetSettingValue<T>(string name)
			where T : struct
		{
			return AsyncUtil.RunSync(() => GetSettingValueAsync<T>(name));
		}

		public T GetSettingValue<T>(string name, long? userId)
			where T : struct
		{
			return AsyncUtil.RunSync(() => GetSettingValueAsync<T>(name, userId));
		}

		public async Task<string> GetSettingValueAsync(string name)
		{
			return await Task.FromResult(GetSettingValue(name, null));
		}

		public async Task<string> GetSettingValueAsync(string name, long? userId)
		{
			return await Task.FromResult(GetSettingValue(name, userId));
		}

		public async Task<T> GetSettingValueAsync<T>(string name) 
			where T : struct
		{
			return await GetSettingValueAsync<T>(name, null);
		}

		public async Task<T> GetSettingValueAsync<T>(string name, long? userId)
            where T : struct
        {
			var r = await Task.FromResult(GetSettingValue(name, userId));
            if (r != null)
            {
                return r.To<T>();
            }
            else
            {
                return default(T);
            }
        }
		
        public string GetSettingValueByHost(string name)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return GetSettingValue(name);
            }
        }

        public T GetSettingValueByHost<T>(string name)
            where T: struct
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return GetSettingValue<T>(name);
            }
        }

        public async Task<string> GetSettingValueByHostAsync(string name)
        {
            return await Task.FromResult(GetSettingValueByHost(name));
        }

        public async Task<T> GetSettingValueByHostAsync<T>(string name)
            where T:struct
        {
            return await Task.FromResult(GetSettingValueByHost<T>(name));
        }

        public string GetSettingValueOrHostSettingValue(string name)
        {
            var resp = GetSettingValue(name);
            if (resp.IsNullOrEmpty())
            {
                resp = GetSettingValueByHost(name);
            }

            return resp;
        }

        public async Task<PasswordComplexityDto> GetPasswordComplexitySettings()
        {
            PasswordComplexityDto pComplexity = null;
            if (CurrentUnitOfWork.GetTenantId().HasValue)
            {
                var settings = await GetAllTenantSettings();
                if (settings.PasswordUseDefaultConfiguration)
                {
                    pComplexity = settings.PasswordComplexityDefault;
                }
                else
                {
                    pComplexity = settings.PasswordComplexity;
                }
            }
            else
            {
                var settings = await GetAllHostSettings();
                if (settings.PasswordUseDefaultConfiguration)
                {
                    pComplexity = settings.PasswordComplexityDefault;
                }
                else
                {
                    pComplexity = settings.PasswordComplexity;
                }
            }

            return pComplexity;
        }

        public long ChangeSetting(string name, string value, long? userId = null)
        {
            return CreateOrUpdateSetting(name, value, userId);            
        }

        public long ChangeSettingByHost(string name, string value)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return ChangeSetting(name, value);
            }
        }

        public long ChangeSettingByUser(string name, string value)
        {
            return ChangeSettingByUser(name, value, SessionContext.UserId);
        }

        public long ChangeSettingByUser(string name, string value, long? user)
        {
            return ChangeSetting(name, value, user);
        }

        #region HOST SETTINGS

        public async Task<HostSettingsDto> GetAllHostSettings()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                PasswordComplexityDto passwordComplexityDefaultDto = await GetPasswordComplexityDefaultAsync();
                string passwordComplexity = await GetSettingValueAsync(AppSettings.Security.PasswordComplexity);
                string passwordComplexityDefault = passwordComplexityDefaultDto.ToJsonString();
                string port = await GetSettingValueAsync(AppSettings.Mail.Smtp.Port);

                return new HostSettingsDto()
                {
                    WebSiteRootAddress = await GetSettingValueAsync(AppSettings.General.WebSiteRootAddress),
                    PasswordUseDefaultConfiguration = passwordComplexity.IsNullOrEmpty() || passwordComplexity == passwordComplexityDefault,
                    PasswordComplexity = passwordComplexity.IsNullOrEmpty() ? passwordComplexityDefaultDto : JsonConvert.DeserializeObject<PasswordComplexityDto>(passwordComplexity),
                    PasswordComplexityDefault = passwordComplexityDefaultDto,
                    EnableUserBlocking = await GetSettingValueAsync<bool>(AppSettings.Security.EnableUserBlocking),
                    FailedAttemptsToBlockUser = await GetSettingValueAsync<byte>(AppSettings.Security.FailedAttemptsToBlockUser),
                    UserBlockingDuration = await GetSettingValueAsync<int>(AppSettings.Security.UserBlockingDuration),
                    EnableTwoFactorLogin = await GetSettingValueAsync<bool>(AppSettings.Security.EnableTwoFactorLogin),
                    EnableMailVerification = await GetSettingValueAsync<bool>(AppSettings.Security.EnableMailVerification),
                    EnableSMSVerification = await GetSettingValueAsync<bool>(AppSettings.Security.EnableSMSVerification),
                    EnableBrowserRemenberMe = await GetSettingValueAsync<bool>(AppSettings.Security.EnableBrowserRemenberMe),
                    MailSMTPSenderDefault = await GetSettingValueAsync(AppSettings.Mail.Smtp.SenderDefault),
                    MailSMTPSenderDefaultDisplayName = await GetSettingValueAsync(AppSettings.Mail.Smtp.SenderDefaultDisplayName),
                    MailSMTPHost = await GetSettingValueAsync(AppSettings.Mail.Smtp.Host),
                    MailSMTPPort = port.IsNullOrEmpty() ? new int?() : Convert.ToInt32(port),
                    MailEnableSSL = await GetSettingValueAsync<bool>(AppSettings.Mail.EnableSSL),
                    MailUseDefaultCredentials = await GetSettingValueAsync<bool>(AppSettings.Mail.UseDefaultCredentials),
                    MailDomainName = await GetSettingValueAsync(AppSettings.Mail.DomainName),
                    MailUserName = await GetSettingValueAsync(AppSettings.Mail.UserName),
                    MailUserPassword = await GetSettingValueAsync(AppSettings.Mail.UserPassword),
                    EmailSendMethod = _emailOptions.Value.SendMethod,
                    GrpcEmail = await GetGrpcEmailSettings()
                };
            }
        }

        public async Task UpdateAllHostSettingsAsync(HostSettingsDto dto)
        {
            if (CurrentUnitOfWork.GetTenantId() != null) {
                throw new NoHostException(L("NoHostExceptionMessage"));
            }

            using (CurrentUnitOfWork.SetTenantId(null))
            {
                ChangeSetting(AppSettings.General.WebSiteRootAddress, dto.WebSiteRootAddress);

                if (dto.PasswordUseDefaultConfiguration)
                {
                    ChangeSetting(AppSettings.Security.PasswordComplexity, (await GetPasswordComplexityDefaultAsync()).ToJsonString());
                }
                else
                {
                    ChangeSetting(AppSettings.Security.PasswordComplexity, dto.PasswordComplexity.ToJsonString());
                }

                ChangeSetting(AppSettings.Security.EnableUserBlocking, dto.EnableUserBlocking.ToString(CultureInfo.InvariantCulture));
                ChangeSetting(AppSettings.Security.FailedAttemptsToBlockUser, dto.FailedAttemptsToBlockUser.ToString(CultureInfo.InvariantCulture));
                ChangeSetting(AppSettings.Security.UserBlockingDuration, dto.UserBlockingDuration.ToString(CultureInfo.InvariantCulture));
                ChangeSetting(AppSettings.Security.EnableTwoFactorLogin, dto.EnableTwoFactorLogin.ToString(CultureInfo.InvariantCulture));
                ChangeSetting(AppSettings.Security.EnableMailVerification, dto.EnableMailVerification.ToString(CultureInfo.InvariantCulture));
                ChangeSetting(AppSettings.Security.EnableSMSVerification, dto.EnableSMSVerification.ToString(CultureInfo.InvariantCulture));
                ChangeSetting(AppSettings.Security.EnableBrowserRemenberMe, dto.EnableBrowserRemenberMe.ToString(CultureInfo.InvariantCulture));

                ChangeSetting(AppSettings.Mail.Smtp.SenderDefault, dto.MailSMTPSenderDefault);
                ChangeSetting(AppSettings.Mail.Smtp.SenderDefaultDisplayName, dto.MailSMTPSenderDefaultDisplayName);
                ChangeSetting(AppSettings.Mail.Smtp.Host, dto.MailSMTPHost);
                ChangeSetting(AppSettings.Mail.Smtp.Port, dto.MailSMTPPort?.ToString(CultureInfo.InvariantCulture));

                ChangeSetting(AppSettings.Mail.EnableSSL, dto.MailEnableSSL.ToString(CultureInfo.InvariantCulture));

                ChangeSetting(AppSettings.Mail.UseDefaultCredentials, dto.MailUseDefaultCredentials.ToString(CultureInfo.InvariantCulture));
                ChangeSetting(AppSettings.Mail.DomainName, dto.MailDomainName);
                ChangeSetting(AppSettings.Mail.UserName, dto.MailUserName);
                ChangeSetting(AppSettings.Mail.UserPassword, dto.MailUserPassword);

                if (_emailOptions.Value.SendMethod == EmailSendMethod.Grpc && !_emailOptions.Value.Grpc.SendConfiguration)
                {
                    ChangeSetting(AppSettings.Mail.GrpcMail.TenancyName, dto.GrpcEmail.TenancyName);
                    ChangeSetting(AppSettings.Mail.GrpcMail.GrpcUserName, dto.GrpcEmail.UserName);
                    ChangeSetting(AppSettings.Mail.GrpcMail.Password, dto.GrpcEmail.Password);
                }
            }
        }

        public async Task<PasswordComplexityDto> GetPasswordComplexityDefaultAsync()
        {
            string passwordComplexity = null;

            if (CurrentUnitOfWork.GetTenantId() == null)
            {
                passwordComplexity = await GetSettingValueByHostAsync(AppSettings.Security.PasswordComplexity);
            }

            if (passwordComplexity.IsNullOrEmpty())
            {
                return new PasswordComplexityDto()
                {
                    MinimumLength = 6,
                    MaximumLength = 10,
                    UseNumbers = true,
                    UseUppercase = false,
                    UseLowercase = true,
                    UsePunctuationSymbols = false
                };
            }
            else
            {
                return JsonConvert.DeserializeObject<PasswordComplexityDto>(passwordComplexity);
            }
        }

        public async Task<PasswordComplexityDto> GetPasswordComplexityAsync()
        {
            string passwordComplexity = await GetSettingValueAsync(AppSettings.Security.PasswordComplexity);

            if (passwordComplexity.IsNullOrEmpty())
            {
                return await GetPasswordComplexityDefaultAsync();
            } else
            {
                return JsonConvert.DeserializeObject<PasswordComplexityDto>(passwordComplexity);
            }
        }

        public async Task SendTestEmail(SendTestEmailDto dto)
        {
            var emailAddress = new EmailAddress { Address = dto.EmailAddress };
            MailTemplateDto mailTemplateDto = new MailTemplateDto()
            {
                Subject = L("TestEmail_Subject"),
                Body = L("TestEmail_Body")
            };

            await Task.Run(() => SendEmail(mailTemplateDto, emailAddress));
        }

        #endregion

        #region TENANT SETTINGS

        public async Task<TenantSettingsDto> GetAllTenantSettings()
        {
            PasswordComplexityDto passwordComplexityDefaultDto = await GetPasswordComplexityDefaultAsync();
            string passwordComplexity = await GetSettingValueAsync(AppSettings.Security.PasswordComplexity);
            string passwordComplexityDefault = passwordComplexityDefaultDto.ToJsonString();
            string port = await GetSettingValueAsync(AppSettings.Mail.Smtp.Port);
            string mailEnableSSL = await GetSettingValueAsync(AppSettings.Mail.EnableSSL);
            string mailUseDefaultCredentials = await GetSettingValueAsync(AppSettings.Mail.UseDefaultCredentials);
            string enablePasswordReuseValidation = await GetSettingValueAsync(AppSettings.Security.EnablePasswordReuseValidation);
            string enablePasswordPeriod = await GetSettingValueAsync(AppSettings.Security.EnablePasswordPeriod);
            string passwordValidDays = await GetSettingValueAsync(AppSettings.Security.PasswordValidDays);
            string preventConcurrentSesions = await GetSettingValueAsync(AppSettings.Security.PreventConcurrentSesions);

            var tSe = new TenantSettingsDto();

            tSe.PasswordUseDefaultConfiguration = passwordComplexity.IsNullOrEmpty() || passwordComplexity == passwordComplexityDefault;
            tSe.PasswordComplexity = passwordComplexity.IsNullOrEmpty() ? passwordComplexityDefaultDto : JsonConvert.DeserializeObject<PasswordComplexityDto>(passwordComplexity);
            tSe.PasswordComplexityDefault = passwordComplexityDefaultDto;

            if (!enablePasswordReuseValidation.IsNullOrEmpty())
            {
                tSe.EnablePasswordReuseValidation = Convert.ToBoolean(enablePasswordReuseValidation);
            }

            if (!enablePasswordPeriod.IsNullOrEmpty())
            {
                tSe.EnablePasswordPeriod = Convert.ToBoolean(enablePasswordPeriod);
            }

            tSe.PasswordValidDays = passwordValidDays.IsNullOrEmpty() ? 0 : Convert.ToInt32(passwordValidDays);
            tSe.EnableUserBlocking = await GetSettingValueAsync<bool>(AppSettings.Security.EnableUserBlocking);
            tSe.FailedAttemptsToBlockUser = await GetSettingValueAsync<byte>(AppSettings.Security.FailedAttemptsToBlockUser);
            tSe.UserBlockingDuration = await GetSettingValueAsync<int>(AppSettings.Security.UserBlockingDuration);

            if (!preventConcurrentSesions.IsNullOrEmpty())
            {
                tSe.PreventConcurrentSesions = Convert.ToBoolean(preventConcurrentSesions);
            }

            tSe.MailSMTPSenderDefault = await GetSettingValueAsync(AppSettings.Mail.Smtp.SenderDefault);
            tSe.MailSMTPSenderDefaultDisplayName = await GetSettingValueAsync(AppSettings.Mail.Smtp.SenderDefaultDisplayName);
            tSe.MailSMTPHost = await GetSettingValueAsync(AppSettings.Mail.Smtp.Host);
            tSe.MailSMTPPort = port.IsNullOrEmpty() ? new int?() : Convert.ToInt32(port);

            if (!mailEnableSSL.IsNullOrEmpty())
            {
                tSe.MailEnableSSL = Convert.ToBoolean(mailEnableSSL);
            }

            tSe.MailUseDefaultCredentials = true;
            if (!mailUseDefaultCredentials.IsNullOrEmpty())
            {
                tSe.MailUseDefaultCredentials = Convert.ToBoolean(mailUseDefaultCredentials);
            }

            tSe.MailDomainName = await GetSettingValueAsync(AppSettings.Mail.DomainName);
            tSe.MailUserName = await GetSettingValueAsync(AppSettings.Mail.UserName);
            tSe.MailUserPassword = await GetSettingValueAsync(AppSettings.Mail.UserPassword);

            tSe.EmailSendMethod = _emailOptions.Value.SendMethod;

            tSe.GrpcEmail = await GetGrpcEmailSettings();

            //Aquí se pueden colocar los Settings que deben extraerse del "host".

            return tSe;
        }

        public async Task<GrpcEmailDto> GetGrpcEmailSettings()
        {
            return new GrpcEmailDto
            {
                SendConfiguration = _emailOptions.Value.Grpc.SendConfiguration,
                TenancyName = await GetSettingValueAsync(AppSettings.Mail.GrpcMail.TenancyName),
                UserName = await GetSettingValueAsync(AppSettings.Mail.GrpcMail.GrpcUserName),
                Password = await GetSettingValueAsync(AppSettings.Mail.GrpcMail.Password),
                Token = await GetSettingValueAsync(AppSettings.Mail.GrpcMail.Token)
            };
        }

        public async Task UpdateAllTenantSettingsAsync(TenantSettingsDto dto)
        {
            if (dto.PasswordUseDefaultConfiguration)
            {
                ChangeSetting(AppSettings.Security.PasswordComplexity, (await GetPasswordComplexityDefaultAsync()).ToJsonString());
            }
            else
            {
                ChangeSetting(AppSettings.Security.PasswordComplexity, dto.PasswordComplexity.ToJsonString());
            }

            ChangeSetting(AppSettings.Security.EnablePasswordReuseValidation, dto.EnablePasswordReuseValidation.ToString(CultureInfo.InvariantCulture));
            ChangeSetting(AppSettings.Security.EnablePasswordPeriod, dto.EnablePasswordPeriod.ToString(CultureInfo.InvariantCulture));
            ChangeSetting(AppSettings.Security.PasswordValidDays, dto.PasswordValidDays.ToString(CultureInfo.InvariantCulture));

            ChangeSetting(AppSettings.Security.EnableUserBlocking, dto.EnableUserBlocking.ToString(CultureInfo.InvariantCulture));
            ChangeSetting(AppSettings.Security.FailedAttemptsToBlockUser, dto.FailedAttemptsToBlockUser.ToString(CultureInfo.InvariantCulture));
            ChangeSetting(AppSettings.Security.UserBlockingDuration, dto.UserBlockingDuration.ToString(CultureInfo.InvariantCulture));

            ChangeSetting(AppSettings.Security.PreventConcurrentSesions, dto.PreventConcurrentSesions.ToString(CultureInfo.InvariantCulture));

            ChangeSetting(AppSettings.Mail.Smtp.SenderDefault, dto.MailSMTPSenderDefault);
            ChangeSetting(AppSettings.Mail.Smtp.SenderDefaultDisplayName, dto.MailSMTPSenderDefaultDisplayName);
            ChangeSetting(AppSettings.Mail.Smtp.Host, dto.MailSMTPHost);
            ChangeSetting(AppSettings.Mail.Smtp.Port, dto.MailSMTPPort?.ToString(CultureInfo.InvariantCulture));

            ChangeSetting(AppSettings.Mail.EnableSSL, dto.MailEnableSSL.ToString(CultureInfo.InvariantCulture));

            ChangeSetting(AppSettings.Mail.UseDefaultCredentials, dto.MailUseDefaultCredentials.ToString(CultureInfo.InvariantCulture));
            ChangeSetting(AppSettings.Mail.DomainName, dto.MailDomainName);
            ChangeSetting(AppSettings.Mail.UserName, dto.MailUserName);
            ChangeSetting(AppSettings.Mail.UserPassword, dto.MailUserPassword);

            if (_emailOptions.Value.SendMethod == EmailSendMethod.Grpc && !_emailOptions.Value.Grpc.SendConfiguration)
            {
                ChangeSetting(AppSettings.Mail.GrpcMail.TenancyName, dto.GrpcEmail.TenancyName);
                ChangeSetting(AppSettings.Mail.GrpcMail.GrpcUserName, dto.GrpcEmail.UserName);
                ChangeSetting(AppSettings.Mail.GrpcMail.Password, dto.GrpcEmail.Password);
            }
        }

		#endregion

		#region Private Methods

		private IQueryable<SettingDto> GetSettingQuery()
		{
			var query = (from entity in _repository.GetAll()
						 select new SettingDto
						 {
							 Id = entity.Id,
							 TenantId = entity.TenantId,
							 UserId = entity.UserId,
							 Name = entity.Name,
							 Value = entity.value
						 });

			return query;
		}

        private long CreateOrUpdateSetting(string name, string value, long? userId = null)
        {
            var sett = _repository.FirstOrDefault(m => m.Name == name && (!userId.HasValue || m.UserId == userId));
            if (sett != null)
            {
                sett.value = value;
            }
            else
            {
                sett = new Setting();
                sett.TenantId = CurrentUnitOfWork.GetTenantId();
                sett.UserId = userId;
                sett.Name = name;
                sett.value = value;

                _repository.Insert(sett);
            }

            CurrentUnitOfWork.SaveChanges();

            return sett.Id;
        }

        #endregion

    }
}
