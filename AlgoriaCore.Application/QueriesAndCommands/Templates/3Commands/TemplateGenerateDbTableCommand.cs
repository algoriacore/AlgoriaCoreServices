using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateGenerateDbTableCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}