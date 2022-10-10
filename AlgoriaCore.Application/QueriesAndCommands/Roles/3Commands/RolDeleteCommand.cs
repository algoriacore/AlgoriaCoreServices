using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands
{
    public class RolDeleteCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}
