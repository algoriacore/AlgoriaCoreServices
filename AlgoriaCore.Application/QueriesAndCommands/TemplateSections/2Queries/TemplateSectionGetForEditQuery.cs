using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionGetForEditQuery : IRequest<TemplateSectionForEditResponse>
    {
        public long? Id { get; set; }
    }
}
