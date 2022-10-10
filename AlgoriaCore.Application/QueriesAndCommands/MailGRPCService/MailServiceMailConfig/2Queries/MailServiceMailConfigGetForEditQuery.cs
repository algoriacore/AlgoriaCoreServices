using MediatR;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailConfigs
{
     public class MailServiceMailConfigGetForEditQuery : IRequest<MailServiceMailConfigForEditResponse>
     {
         public long Id { get; set; }
     }
}

