using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateDeleteCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}