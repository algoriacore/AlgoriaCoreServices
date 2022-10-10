using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusGetForEditQuery : IRequest<TemplateToDoStatusForEditResponse>
    {
        public long? Id { get; set; }
    }
}
