using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessDeleteCommand : IRequest<long>
    {
        public long Template { get; set; }
        public long Id { get; set; }
    }
}