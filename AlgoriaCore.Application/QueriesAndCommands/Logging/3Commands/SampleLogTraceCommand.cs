using AlgoriaCore.Domain.Attributes;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Logging._3Commands
{
    [Auditable(false)]
    public class SampleLogTraceCommand : IRequest<bool>
    {
        public string Message { get; set; }
    }
}
