using AlgoriaCore.Application.Configuration;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Tenant
{
    public class TenantSettingsForEditResponse
    {
        public bool PasswordUseDefaultConfiguration { get; set; }
        public TenantSettingsPasswordComplexityResponse PasswordComplexity { get; set; }
        public TenantSettingsPasswordComplexityResponse PasswordComplexityDefault { get; set; }
        public bool EnablePasswordReuseValidation { get; set; }
        public bool EnablePasswordPeriod { get; set; }
        public int PasswordValidDays { get; set; }
        public bool EnableUserBlocking { get; set; }
        public byte FailedAttemptsToBlockUser { get; set; }
        public bool PreventConcurrentSesions { get; set; }
        public int UserBlockingDuration { get; set; }
        public string MailSMTPSenderDefault { get; set; }
        public string MailSMTPSenderDefaultDisplayName { get; set; }
        public string MailSMTPHost { get; set; }
        public int? MailSMTPPort { get; set; }
        public bool MailEnableSSL { get; set; }
        public bool MailUseDefaultCredentials { get; set; }
        public string MailDomainName { get; set; }
        public string MailUserName { get; set; }
        public string MailUserPassword { get; set; }
        public EmailSendMethod EmailSendMethod { get; set; }
        public GrpcEmailResponse GrpcEmail { get; set; }
    }

    public class TenantSettingsPasswordComplexityResponse
    {
        public byte MinimumLength { get; set; }
        public byte MaximumLength { get; set; }
        public bool UseNumbers { get; set; }
        public bool UseUppercase { get; set; }
        public bool UseLowercase { get; set; }
        public bool UsePunctuationSymbols { get; set; }
    }

    public class GrpcEmailResponse
    {
        public bool SendConfiguration { get; set; }
        public string TenancyName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}