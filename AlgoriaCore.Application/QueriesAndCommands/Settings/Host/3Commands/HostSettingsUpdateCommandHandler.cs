using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Application.Managers.Settings.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Host
{
    public class HostSettingsUpdateCommandHandler : BaseCoreClass, IRequestHandler<HostSettingsUpdateCommand, int>
    {
        private readonly SettingManager _managerSetting;

        public HostSettingsUpdateCommandHandler(ICoreServices coreServices
        , SettingManager managerSetting) : base(coreServices)
        {
            _managerSetting = managerSetting;
        }

        public async Task<int> Handle(HostSettingsUpdateCommand request, CancellationToken cancellationToken)
        {
            HostSettingsDto dto = new HostSettingsDto()
            {
                WebSiteRootAddress = request.WebSiteRootAddress,
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
                EnableUserBlocking = request.EnableUserBlocking,
                FailedAttemptsToBlockUser = request.FailedAttemptsToBlockUser,
                UserBlockingDuration = request.UserBlockingDuration,
                EnableTwoFactorLogin = request.EnableTwoFactorLogin,
                EnableMailVerification = request.EnableMailVerification,
                EnableSMSVerification = request.EnableSMSVerification,
                EnableBrowserRemenberMe = request.EnableBrowserRemenberMe,
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

            await _managerSetting.UpdateAllHostSettingsAsync(dto);

            return 0;
        }
    }
}
