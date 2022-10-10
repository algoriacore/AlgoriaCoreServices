using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.Processes.Dto
{
    public class ProcessDto
    {
        public long? Id { get; set; }
        public Dictionary<string, string> DataFromClient { get; set; }
        public Dictionary<string, object> DataFromServer { get; set; }

        public ToDoActivityDto Activity { get; set; }

        public ProcessDto() {
            DataFromClient = new Dictionary<string, string>();
            DataFromServer = new Dictionary<string, object>();
        }
    }

    public class ToDoActivityDto
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
        public string Table { get; set; }
        public long? Key { get; set; }

        public List<ToDoActivityUserDto> Evaluator { get; set; }
        public List<ToDoActivityUserDto> Executor { get; set; }

        public ToDoActivityDto() {
            Evaluator = new List<ToDoActivityUserDto>();
            Executor = new List<ToDoActivityUserDto>();
        }
    }

    public class ToDoActivityUserDto
    {
        public long? Id { get; set; }
        public long? ToDoActivity { get; set; }
        public long User { get; set; }
        public string UserDesc { get; set; }
    }

    public class ToDoTimeSheetDto
    {
        public long? Id { get; set; }
        public long Activity { get; set; }
        public long UserCreator { get; set; }
        public string UserCreatorDesc { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Comments { get; set; }
        public decimal HoursSpend { get; set; }
        public long? ActivityStatus { get; set; }
    }
}
