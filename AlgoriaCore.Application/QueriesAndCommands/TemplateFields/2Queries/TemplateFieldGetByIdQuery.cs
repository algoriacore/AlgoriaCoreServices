using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldGetByIdQuery : IRequest<TemplateFieldResponse>
    {
        public long Id { get; set; }
    }
}
