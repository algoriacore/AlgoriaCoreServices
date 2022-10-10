using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionDeleteCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}