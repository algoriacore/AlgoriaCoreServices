using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserDeleteCommandHandler : BaseCoreClass, IRequestHandler<UserDeleteCommand, long>
    {
        private readonly UserManager _manager;

        public UserDeleteCommandHandler(ICoreServices coreServices, UserManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            return await _manager.DeleteUserAsync(request.Id);
        }
    }
}
