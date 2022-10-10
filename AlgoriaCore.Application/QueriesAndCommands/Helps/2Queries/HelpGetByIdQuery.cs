using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpGetByIdQuery : IRequest<HelpResponse>
    {
        public long Id { get; set; }
    }
}
