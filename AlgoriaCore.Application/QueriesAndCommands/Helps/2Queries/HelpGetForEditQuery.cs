using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpGetForEditQuery : IRequest<HelpForEditResponse>
    {
        public long? Id { get; set; }
    }
}
