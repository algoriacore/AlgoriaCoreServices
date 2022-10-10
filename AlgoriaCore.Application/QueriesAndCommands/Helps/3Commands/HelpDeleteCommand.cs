using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpDeleteCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}