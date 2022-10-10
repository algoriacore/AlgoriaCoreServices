using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessResponse
    {
        public long? Id { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public ProcessToDoActivityResponse Activity { get; set; }
    }
}