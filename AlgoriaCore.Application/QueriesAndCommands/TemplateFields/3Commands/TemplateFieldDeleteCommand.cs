using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldDeleteCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}