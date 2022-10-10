using System;
namespace AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailAttachs.Dto
{
     public class MailServiceMailAttachDto
     {
         public long? MailServiceMailBody { get; set; }
         public string MailServiceMailBodyDesc { get; set; }
         public string ContenType { get; set; }
         public string FileName { get; set; }
         public byte[] BinaryFile { get; set; }
         public long Id { get; set; }
     }
}

