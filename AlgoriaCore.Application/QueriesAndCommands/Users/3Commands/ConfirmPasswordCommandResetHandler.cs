using AlgoriaCore.Application.Managers.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class ConfirmPasswordCommandResetHandler : IRequestHandler<ConfirmPasswordCommandReset, long>
    {
        private readonly UserManager _userManager;

        public ConfirmPasswordCommandResetHandler(UserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<long> Handle(ConfirmPasswordCommandReset request, CancellationToken cancellationToken)
        {
            using (_userManager.CurrentUnitOfWork.SetTenantId(null))
            {
                var x = await _userManager.ChangePasswordFromResetCodeAsync(request.ConfirmationCode, request.Password);
                return x;
            }
        }
    }
}
