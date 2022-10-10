using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusDeleteCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}