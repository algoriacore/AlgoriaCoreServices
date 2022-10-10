using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserUnlockCommandHandler : BaseCoreClass, IRequestHandler<UserUnlockCommand, long>
    {
        private readonly UserManager _manager;

        public UserUnlockCommandHandler(ICoreServices coreServices, UserManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(UserUnlockCommand request, CancellationToken cancellationToken)
        {
            await _manager.UnlockUserAsync(request.Id);

            return request.Id;
        }
    }
}
