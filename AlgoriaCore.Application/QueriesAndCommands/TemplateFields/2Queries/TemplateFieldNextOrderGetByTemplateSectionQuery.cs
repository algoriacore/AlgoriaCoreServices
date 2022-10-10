using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldNextOrderGetByTemplateSectionQuery : IRequest<short>
    {
        public long TemplateSection { get; set; }
    }
}
