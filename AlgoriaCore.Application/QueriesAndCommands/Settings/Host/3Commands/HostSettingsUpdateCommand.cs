using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Host
{
    public class HostSettingsUpdateCommand : IRequest<int>
    {
        public string WebSiteRootAddress { get; set; }
        public bool PasswordUseDefaultConfiguration { get; set; }
        public HostSettingsPasswordComplexityCommand PasswordComplexity { get; set; }
        public bool EnableUserBlocking { get; set; }
        public byte FailedAttemptsToBlockUser { get; set; }
        public int UserBlockingDuration { get; set; }
        public bool EnableTwoFactorLogin { get; set; }
        public bool EnableMailVerification { get; set; }
        public bool EnableSMSVerification { get; set; }
        public bool EnableBrowserRemenberMe { get; set; }
        public string MailSMTPSenderDefault { get; set; }
        public string MailSMTPSenderDefaultDisplayName { get; set; }
        public string MailSMTPHost { get; set; }
        public int? MailSMTPPort { get; set; }
        public bool MailEnableSSL { get; set; }
        public bool MailUseDefaultCredentials { get; set; }
        public string MailDomainName { get; set; }
        public string MailUserName { get; set; }
        public string MailUserPassword { get; set; }
        public HostGrpcEmailCommand GrpcEmail { get; set; }
        public HostSettingsUpdateCommand() 
        {
            PasswordComplexity = new HostSettingsPasswordComplexityCommand();
            GrpcEmail = new HostGrpcEmailCommand();
        }
    }

    public class HostSettingsPasswordComplexityCommand
    {
        public byte MinimumLength { get; set; }
        public byte MaximumLength { get; set; }
        public bool UseNumbers { get; set; }
        public bool UseUppercase { get; set; }
        public bool UseLowercase { get; set; }
        public bool UsePunctuationSymbols { get; set; }
    }

    public class HostGrpcEmailCommand
    {
        public string TenancyName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}