using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailAttachs
{
     public class MailServiceMailAttachGetListQuery : PageListByDto, IRequest<PagedResultDto<MailServiceMailAttachListResponse>>
     {
        public long? MailServiceMailBody { get; set; }
     }
}

