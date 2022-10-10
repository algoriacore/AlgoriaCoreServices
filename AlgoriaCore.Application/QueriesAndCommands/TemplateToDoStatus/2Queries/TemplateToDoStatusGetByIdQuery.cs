using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusGetByIdQuery : IRequest<TemplateToDoStatusResponse>
    {
        public long Id { get; set; }
    }
}
