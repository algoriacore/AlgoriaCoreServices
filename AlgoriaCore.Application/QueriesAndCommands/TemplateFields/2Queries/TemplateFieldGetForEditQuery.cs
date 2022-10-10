using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldGetForEditQuery : IRequest<TemplateFieldForEditResponse>
    {
        public long? Id { get; set; }
    }
}
