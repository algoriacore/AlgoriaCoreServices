using MediatR;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailAttachs
{
     public class MailServiceMailAttachGetForEditQuery : IRequest<MailServiceMailAttachForEditResponse>
     {
         public long Id { get; set; }
     }
}

