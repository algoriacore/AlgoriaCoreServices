using System;
namespace AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailStatuss.Dto
{
     public class MailServiceMailStatusDto
     {
         public long MailServiceMail { get; set; }
         public string MailServiceMailDesc { get; set; }
         public DateTime? SentTime { get; set; }
         public byte Status { get; set; }
         public string StatusDesc { get; set; }
         public string Error { get; set; }
         public long Id { get; set; }
     }
}

