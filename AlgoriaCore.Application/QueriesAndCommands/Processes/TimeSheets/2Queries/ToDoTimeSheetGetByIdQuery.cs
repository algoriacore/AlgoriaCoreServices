using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetGetByIdQuery : IRequest<ToDoTimeSheetResponse>
    {
        public long Id { get; set; }
    }
}
