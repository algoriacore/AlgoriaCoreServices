using AlgoriaCore.Application.BaseClases.Dto;
namespace AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailAttachs.Dto
{
     public class MailServiceMailAttachListFilterDto: PageListByDto
     {
        public long? MailServiceMailBody { get; set; }
    }
}

