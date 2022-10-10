using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetForReadQuery : IRequest<ProcessForReadResponse>
    {
        public long Template { get; set; }
        public long Id { get; set; }
    }
}
