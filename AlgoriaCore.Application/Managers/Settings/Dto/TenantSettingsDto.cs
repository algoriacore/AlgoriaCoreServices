using AlgoriaCore.Application.Configuration;

namespace AlgoriaCore.Application.Managers.Settings.Dto
{
    public class TenantSettingsDto
    {
        public bool PasswordUseDefaultConfiguration { get; set; }
        public PasswordComplexityDto PasswordComplexityDefault { get; set; }
        public PasswordComplexityDto PasswordComplexity { get; set; }
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
        public EmailSendMethod EmailSendMethod { get; set; }
        public GrpcEmailDto GrpcEmail { get; set; }
    }
}
