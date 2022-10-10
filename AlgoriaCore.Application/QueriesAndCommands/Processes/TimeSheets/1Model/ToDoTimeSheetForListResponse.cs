using System;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetForListResponse
    {
        public long? Id { get; set; }
        public long Activity { get; set; }
        public long UserCreator { get; set; }
        public string UserCreatorDesc { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Comments { get; set; }
        public decimal HoursSpend { get; set; }
    }
}