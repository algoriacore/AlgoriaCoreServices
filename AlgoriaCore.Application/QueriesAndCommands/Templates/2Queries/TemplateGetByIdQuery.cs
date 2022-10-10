using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateGetByIdQuery : IRequest<TemplateResponse>
    {
        public long Id { get; set; }
    }
}
