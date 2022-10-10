using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetByIdQuery : IRequest<ProcessResponse>
    {
        public long Template { get; set; }
        public long Id { get; set; }
    }
}
