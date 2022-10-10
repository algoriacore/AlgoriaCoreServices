using MediatR;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailAttachs
{
     public class MailServiceMailAttachGetFileQuery : IRequest<MailServiceMailAttachForEditResponse>
     {
         public long Id { get; set; }
     }
}

