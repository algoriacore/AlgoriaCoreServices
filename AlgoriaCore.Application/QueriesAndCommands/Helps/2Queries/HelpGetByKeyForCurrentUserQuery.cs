using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpGetByKeyForCurrentUserQuery : IRequest<HelpResponse>
    {
        public string Key { get; set; }
    }
}
