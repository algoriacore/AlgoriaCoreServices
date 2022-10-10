using System;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMails
{
    public class MailServiceMailListResponse
    {
        public long Id { get; set; }
        public long MailServiceRequest { get; set; }
        public DateTime? MailServiceRequestDate { get; set; }
        public bool IsLocalConfig { get; set; }
        public string IsLocalConfigDesc { get; set; }
        public string Sendto { get; set; }
        public string CopyTo { get; set; }
        public string Subject { get; set; }
        public byte Status { get; set; }
        public string StatusDesc { get; set; }
    }
}

