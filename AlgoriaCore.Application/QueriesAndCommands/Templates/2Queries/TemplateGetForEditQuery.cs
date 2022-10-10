using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateGetForEditQuery : IRequest<TemplateForEditResponse>
    {
        public long? Id { get; set; }
    }
}
