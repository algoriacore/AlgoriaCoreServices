namespace AlgoriaCore.Domain.Settings
{
    public static class AppSettings
    {
        public static class General
        {
            public const string LanguageDefault = "LanguageDefault";
            public const string MailGroup = "MailGroup";
            public const string WebSiteRootAddress = "WebSiteRootAddress";
        }

        public static class Security
        {
            public const string PasswordComplexity = "PasswordComplexity";
            public const string EnablePasswordReuseValidation = "EnablePasswordReuseValidation";
            public const string EnablePasswordPeriod = "EnablePasswordPeriod";
            public const string EnableUserBlocking = "EnableUserBlocking";
            public const string PasswordValidDays = "PasswordValidDays";
            public const string FailedAttemptsToBlockUser = "FailedAttemptsToBlockUser";
            public const string UserBlockingDuration = "UserBlockingDuration";
            public const string PreventConcurrentSesions = "PreventConcurrentSesions";
            public const string EnableTwoFactorLogin = "EnableTwoFactorLogin";
            public const string EnableMailVerification = "EnableMailVerification";
            public const string EnableSMSVerification = "EnableSMSVerification";
            public const string EnableBrowserRemenberMe = "EnableBrowserRemenberMe";
        }

        public static class Mail
        {
            public const string EnableSSL = "MailEnableSSL";
            public const string UseDefaultCredentials = "MailUseDefaultCredentials";
            public const string DomainName = "MailDomainName";
            public const string UserName = "MailUserName";
            public const string UserPassword = "MailUserPassword";

            public static class Smtp
            {
                public const string SenderDefault = "MailSMTPSenderDefault";
                public const string SenderDefaultDisplayName = "MailSMTPSenderDefaultDisplayName";
                public const string Host = "MailSMTPHost";
                public const string Port = "MailSMTPPort";
            }

            public static class Pop
            {

            }

            public static class GrpcMail
            {
                public const string TenancyName = "GrpcMailTenancyName";
                public const string GrpcUserName = "GrpcMailUserName";
                public const string Password = "GrpcMailPassword";
                public const string Token = "GrpcMailToken";
            }
        }
    }
}
