using MediatR;
using System;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetCreateCommand : IRequest<long>
    {
        public long Template { get; set; }
        public long Activity { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Comments { get; set; }
        public decimal HoursSpend { get; set; }
        public long? ActivityStatus { get; set; }
    }
}