using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionGetByIdQuery : IRequest<TemplateSectionResponse>
    {
        public long Id { get; set; }
    }
}
