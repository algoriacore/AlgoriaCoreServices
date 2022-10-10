using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Tenant
{
    public class TenantSettingsUpdateCommand : IRequest<int>
    {
        public bool PasswordUseDefaultConfiguration { get; set; }
        public TenantSettingsPasswordComplexityCommand PasswordComplexity { get; set; }
        public bool EnablePasswordReuseValidation { get; set; }
        public bool EnablePasswordPeriod { get; set; }
        public int PasswordValidDays { get; set; }
        public bool EnableUserBlocking { get; set; }
        public byte FailedAttemptsToBlockUser { get; set; }
        public int UserBlockingDuration { get; set; }
        public bool PreventConcurrentSesions { get; set; }
        public string MailSMTPSenderDefault { get; set; }
        public string MailSMTPSenderDefaultDisplayName { get; set; }
        public string MailSMTPHost { get; set; }
        public int? MailSMTPPort { get; set; }
        public bool MailEnableSSL { get; set; }
        public bool MailUseDefaultCredentials { get; set; }
        public string MailDomainName { get; set; }
        public string MailUserName { get; set; }
        public string MailUserPassword { get; set; }
        public GrpcEmailCommand GrpcEmail { get; set; }

        public TenantSettingsUpdateCommand() 
        {
            PasswordComplexity = new TenantSettingsPasswordComplexityCommand();
            GrpcEmail = new GrpcEmailCommand();
        }
    }

    public class TenantSettingsPasswordComplexityCommand
    {
        public byte MinimumLength { get; set; }
        public byte MaximumLength { get; set; }
        public bool UseNumbers { get; set; }
        public bool UseUppercase { get; set; }
        public bool UseLowercase { get; set; }
        public bool UsePunctuationSymbols { get; set; }
    }

    public class GrpcEmailCommand
    {
        public string TenancyName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}