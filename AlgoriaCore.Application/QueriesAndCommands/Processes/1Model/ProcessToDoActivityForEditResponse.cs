using AlgoriaCore.Application.Managers.Processes.Dto;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessToDoActivityForEditResponse
    {
        public long? Id { get; set; }
        public long UserCreator { get; set; }
        public string UserCreatorDesc { get; set; }
        public long Status { get; set; }
        public string StatusDesc { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Description { get; set; }
        public DateTime? InitialPlannedDate { get; set; }
        public DateTime? InitialRealDate { get; set; }
        public DateTime? FinalPlannedDate { get; set; }
        public DateTime? FinalRealDate { get; set; }
        public bool IsOnTime { get; set; }
        public string IsOnTimeDesc { get; set; }

        public List<ToDoActivityUserDto> Evaluator { get; set; }
        public List<ToDoActivityUserDto> Executor { get; set; }
    }
}