using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserLockCommandHandler : BaseCoreClass, IRequestHandler<UserLockCommand, long>
    {
        private readonly UserManager _manager;

        public UserLockCommandHandler(ICoreServices coreServices, UserManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(UserLockCommand request, CancellationToken cancellationToken)
        {
            await _manager.LockUserAsync(request.Id);

            return request.Id;
        }
    }
}
