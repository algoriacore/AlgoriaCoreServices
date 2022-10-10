using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessToDoActivityCommand
    {
        public long Status { get; set; }
        public string Description { get; set; }
        public DateTime? InitialPlannedDate { get; set; }
        public DateTime? InitialRealDate { get; set; }
        public DateTime? FinalPlannedDate { get; set; }
        public DateTime? FinalRealDate { get; set; }

        public List<long> Evaluator { get; set; }
        public List<long> Executor { get; set; }

        public ProcessToDoActivityCommand() {
            Evaluator = new List<long>();
            Executor = new List<long>();
        }
    }
}