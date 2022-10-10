using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Application.Managers.Settings.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Tenant
{
    public class TenantSettingsGetForEditQueryHandler : BaseCoreClass, IRequestHandler<TenantSettingsGetForEditQuery, TenantSettingsForEditResponse>
    {
        private readonly SettingManager _managerSetting;
        
        public TenantSettingsGetForEditQueryHandler(
            ICoreServices coreServices,
            SettingManager managerSetting
        ) : base(coreServices)
        {
            _managerSetting = managerSetting;
        }

        public async Task<TenantSettingsForEditResponse> Handle(TenantSettingsGetForEditQuery request, CancellationToken cancellationToken)
        {
            TenantSettingsDto dto = await _managerSetting.GetAllTenantSettings();

            return new TenantSettingsForEditResponse()
            {
                PasswordUseDefaultConfiguration = dto.PasswordUseDefaultConfiguration,
                PasswordComplexity = new TenantSettingsPasswordComplexityResponse()
                {
                    MinimumLength = dto.PasswordComplexity.MinimumLength,
                    MaximumLength = dto.PasswordComplexity.MaximumLength,
                    UseNumbers = dto.PasswordComplexity.UseNumbers,
                    UseUppercase = dto.PasswordComplexity.UseUppercase,
                    UseLowercase = dto.PasswordComplexity.UseLowercase,
                    UsePunctuationSymbols = dto.PasswordComplexity.UsePunctuationSymbols
                },
                PasswordComplexityDefault = new TenantSettingsPasswordComplexityResponse()
                {
                    MinimumLength = dto.PasswordComplexityDefault.MinimumLength,
                    MaximumLength = dto.PasswordComplexityDefault.MaximumLength,
                    UseNumbers = dto.PasswordComplexityDefault.UseNumbers,
                    UseUppercase = dto.PasswordComplexityDefault.UseUppercase,
                    UseLowercase = dto.PasswordComplexityDefault.UseLowercase,
                    UsePunctuationSymbols = dto.PasswordComplexityDefault.UsePunctuationSymbols
                },
                EnablePasswordReuseValidation = dto.EnablePasswordReuseValidation,
                EnablePasswordPeriod = dto.EnablePasswordPeriod,
                PasswordValidDays = dto.PasswordValidDays,
                EnableUserBlocking = dto.EnableUserBlocking,
                FailedAttemptsToBlockUser = dto.FailedAttemptsToBlockUser,
                UserBlockingDuration = dto.UserBlockingDuration,
                PreventConcurrentSesions = dto.PreventConcurrentSesions,
                MailSMTPSenderDefault = dto.MailSMTPSenderDefault,
                MailSMTPSenderDefaultDisplayName = dto.MailSMTPSenderDefaultDisplayName,
                MailSMTPHost = dto.MailSMTPHost,
                MailSMTPPort = dto.MailSMTPPort,
                MailEnableSSL = dto.MailEnableSSL,
                MailUseDefaultCredentials = dto.MailUseDefaultCredentials,
                MailDomainName = dto.MailDomainName,
                MailUserName = dto.MailUserName,
                MailUserPassword = dto.MailUserPassword,
                EmailSendMethod = dto.EmailSendMethod,
                GrpcEmail = new GrpcEmailResponse
                {
                    SendConfiguration = dto.GrpcEmail.SendConfiguration,
                    TenancyName = dto.GrpcEmail.TenancyName,
                    UserName = dto.GrpcEmail.UserName,
                    Password = dto.GrpcEmail.Password
                }
            };
        }
    }
}
