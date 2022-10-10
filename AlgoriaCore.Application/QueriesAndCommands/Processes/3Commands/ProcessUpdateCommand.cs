using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessUpdateCommand : IRequest<long>
    {
        public long Template { get; set; }
        public long Id { get; set; }
        public Dictionary<string, string> Data { get; set; }

        public ProcessToDoActivityCommand Activity { get; set; }
    }
}