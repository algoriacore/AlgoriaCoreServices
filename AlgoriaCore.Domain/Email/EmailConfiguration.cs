using AlgoriaCore.Domain.Interfaces.Email;

namespace AlgoriaCore.Domain.Email
{
    public class EmailConfiguration : IEmailServiceConfiguration
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

        public string PopServer { get; }

        public int PopPort { get; }

        public string PopUsername { get; }

        public string PopPassword { get; }
    }
}
