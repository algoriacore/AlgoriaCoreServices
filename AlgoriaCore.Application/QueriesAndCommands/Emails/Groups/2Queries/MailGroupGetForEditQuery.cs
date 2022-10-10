using AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._2Queries
{
    public class MailGroupGetForEditQuery : IRequest<MailGroupForEditResponse>
    {
        public long Id { get; set; }
    }
}
