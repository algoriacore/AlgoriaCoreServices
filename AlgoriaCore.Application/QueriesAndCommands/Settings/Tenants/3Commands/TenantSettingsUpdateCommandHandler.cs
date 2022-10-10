using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Application.Managers.Settings.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Tenant
{
    public class TenantSettingsUpdateCommandHandler : BaseCoreClass, IRequestHandler<TenantSettingsUpdateCommand, int>
    {
        private readonly SettingManager _managerSetting;

        public TenantSettingsUpdateCommandHandler(ICoreServices coreServices
        , SettingManager managerSetting) : base(coreServices)
        {
            _managerSetting = managerSetting;
        }

        public async Task<int> Handle(TenantSettingsUpdateCommand request, CancellationToken cancellationToken)
        {
            TenantSettingsDto dto = new TenantSettingsDto()
            {
                PasswordUseDefaultConfiguration = request.PasswordUseDefaultConfiguration,
                PasswordComplexity = new PasswordComplexityDto()
                {
                    MinimumLength = request.PasswordComplexity.MinimumLength,
                    MaximumLength = request.PasswordComplexity.MaximumLength,
                    UseNumbers = request.PasswordComplexity.UseNumbers,
                    UseUppercase = request.PasswordComplexity.UseUppercase,
                    UseLowercase = request.PasswordComplexity.UseLowercase,
                    UsePunctuationSymbols = request.PasswordComplexity.UsePunctuationSymbols
                },
                EnablePasswordReuseValidation = request.EnablePasswordReuseValidation,
                EnablePasswordPeriod = request.EnablePasswordPeriod,
                PasswordValidDays = request.PasswordValidDays,
                EnableUserBlocking = request.EnableUserBlocking,
                FailedAttemptsToBlockUser = request.FailedAttemptsToBlockUser,
                UserBlockingDuration = request.UserBlockingDuration,
                PreventConcurrentSesions = request.PreventConcurrentSesions,
                MailSMTPSenderDefault = request.MailSMTPSenderDefault,
                MailSMTPSenderDefaultDisplayName = request.MailSMTPSenderDefaultDisplayName,
                MailSMTPHost = request.MailSMTPHost,
                MailSMTPPort = request.MailSMTPPort,
                MailEnableSSL = request.MailEnableSSL,
                MailUseDefaultCredentials = request.MailUseDefaultCredentials,
                MailDomainName = request.MailDomainName,
                MailUserName = request.MailUserName,
                MailUserPassword = request.MailUserPassword,
                GrpcEmail = new GrpcEmailDto
                {
                    TenancyName = request.GrpcEmail.TenancyName,
                    UserName = request.GrpcEmail.UserName,
                    Password = request.GrpcEmail.Password
                }
            };

            await _managerSetting.UpdateAllTenantSettingsAsync(dto);

            return 0;
        }
    }
}
