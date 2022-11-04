namespace AlgoriaCore.Application.Managers.Emails.Templates.Dto
{
    public class MailTemplateDto
    {
        public long? Id { get; set; }
        public int? TenantId { get; set; }
        public long? MailGroup { get; set; }
        public string MailKey { get; set; }
        public string MailKeyDesc { get; set; }
        public string DisplayName { get; set; }
        public string SendTo { get; set; }
        public string CopyTo { get; set; }
        public string BlindCopyTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }

        public string Header { get; set; }
        public string Footer { get; set; }
    }
}
