using AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._2Queries
{
    public class MailTemplateGetForEditQuery : IRequest<MailTemplateForEditResponse>
    {
        public long Id { get; set; }
    }
}
