using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetGetForEditQuery : IRequest<ToDoTimeSheetForEditResponse>
    {
        public long? Id { get; set; }
        public long Template { get; set; }
    }
}
