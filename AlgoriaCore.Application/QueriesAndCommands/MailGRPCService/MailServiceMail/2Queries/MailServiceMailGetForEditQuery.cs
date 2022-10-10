using MediatR;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMails
{
     public class MailServiceMailGetForEditQuery : IRequest<MailServiceMailForEditResponse>
     {
         public long Id { get; set; }
     }
}

