using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpCreateCommand : IRequest<long>
    {
        public int Language { get; set; }
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
    }
}