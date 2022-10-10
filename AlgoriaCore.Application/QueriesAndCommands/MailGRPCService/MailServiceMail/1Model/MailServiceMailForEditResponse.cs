using AlgoriaCore.Application.BaseClases.Dto;
using System.Collections.Generic;
using System;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailStatuss.Dto;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailBodys.Dto;

namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMails
{
    public class MailServiceMailForEditResponse
    {
        public long? Id { get; set; }
        public long? MailServiceRequest { get; set; }
        public DateTime? MailServiceRequestDate { get; set; }
        public bool? IsLocalConfig { get; set; }
        public string IsLocalConfigDesc { get; set; }
        public string Sendto { get; set; }
        public string CopyTo { get; set; }
        public string BlindCopyTo { get; set; }
        public string Subject { get; set; }

        public MailServiceMailStatusDto MailServiceMailStatus { get; set; }
        public MailServiceMailBodyDto MailServiceMailBody { get; set; }

    }
}

