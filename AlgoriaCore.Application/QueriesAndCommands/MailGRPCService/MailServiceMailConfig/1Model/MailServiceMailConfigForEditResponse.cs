using AlgoriaCore.Application.BaseClases.Dto;
using System.Collections.Generic;
using System;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailConfigs
{
     public class MailServiceMailConfigForEditResponse
     {
         public long MailServiceMail { get; set; }
         public string MailServiceMailDesc { get; set; }
         public string Sender { get; set; }
         public string SenderDisplay { get; set; }
         public string Smpthost { get; set; }
         public short? Smptport { get; set; }
         public bool? IsSsl { get; set; }
         public string IsSslDesc { get; set; }
         public bool? UseDefaultCredential { get; set; }
         public string UseDefaultCredentialDesc { get; set; }
         public string Domain { get; set; }
         public string MailUser { get; set; }
         public string MailPassword { get; set; }
         public long Id { get; set; }
     }
}

