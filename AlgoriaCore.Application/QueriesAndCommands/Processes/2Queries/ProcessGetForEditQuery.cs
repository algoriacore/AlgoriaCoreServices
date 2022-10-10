using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetForEditQuery : IRequest<ProcessForEditResponse>
    {
        public long Template { get; set; }
        public long? Id { get; set; }
    }
}
