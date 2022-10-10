using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessCreateCommand : IRequest<long>
    {
        public long Template { get; set; }
        public Dictionary<string, string> Data { get; set; }

        public ProcessToDoActivityCommand Activity { get; set; }
    }
}