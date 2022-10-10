using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserLockCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}
