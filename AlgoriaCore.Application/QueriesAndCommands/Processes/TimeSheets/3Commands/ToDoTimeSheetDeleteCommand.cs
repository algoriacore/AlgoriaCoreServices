using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetDeleteCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}