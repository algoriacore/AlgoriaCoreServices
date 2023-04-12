using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands
{
    public class RoleDeleteCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}
