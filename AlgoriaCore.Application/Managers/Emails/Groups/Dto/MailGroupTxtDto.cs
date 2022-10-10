namespace AlgoriaCore.Application.Managers.Emails.Groups.Dto
{
    public class MailGroupTxtDto
    {
        public long? Id { get; set; }
        public long? MailGroup { get; set; }
        public MailGroupTxtType Type { get; set; }
        public string Body { get; set; }
    }

    public enum MailGroupTxtType
    {
        Header = 1,
        Footer = 2
    }
}
