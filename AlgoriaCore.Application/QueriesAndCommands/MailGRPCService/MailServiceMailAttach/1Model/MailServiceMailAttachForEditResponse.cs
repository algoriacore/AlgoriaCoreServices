using AlgoriaCore.Application.BaseClases.Dto;
using System.Collections.Generic;
using System;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailAttachs
{
    public class MailServiceMailAttachForEditResponse
    {
        public long Id { get; set; }
        public long? MailServiceMailBody { get; set; }
        public string MailServiceMailBodyDesc { get; set; }
        public string ContenType { get; set; }
        public string FileName { get; set; }
        public string Base64File { get; set; }
    }
}

