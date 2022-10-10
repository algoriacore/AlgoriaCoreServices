using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Interfaces.MultiTenancy;
using AlgoriaCore.Extensions;
using AlgoriaPersistence.Interfaces.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserResetPasswordCommandHandler : BaseCoreClass, IRequestHandler<UserResetPasswordCommand, string>
    {
		private readonly TenantManager _tenantManager;
		private readonly UserManager _userManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public UserResetPasswordCommandHandler(ICoreServices coreServices,
                                        UserManager userManager,
                                        TenantManager tenantManager,
                                        IMultiTenancyConfig multiTenancyConfig) : base(coreServices)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _multiTenancyConfig = multiTenancyConfig;

        }

        public async Task<string> Handle(UserResetPasswordCommand request, CancellationToken cancellationToken)
		{
			string x = null;
			int? tId = null;

            if(!_multiTenancyConfig.IsEnabled()) {
                request.TenancyName = _multiTenancyConfig.GetTenancyNameDefault();
            }

            if (!request.TenancyName.IsNullOrEmpty())
			{
				using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
				{
					using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
					{
						var tenant = await _tenantManager.GetTenantByTenancyNameAsync(request.TenancyName);
						if (tenant == null)
						{
							throw new AlgoriaCoreGeneralException(L("ChangePassword.WrongTenancyName"));
						}

						tId = tenant.Id;
					}
				}
			}

			using (_userManager.CurrentUnitOfWork.SetTenantId(tId))
			{
				x = await _userManager.CreateResetPasswordCodeAsync(request.UserName, request.TenancyName);
			}

			return x;
		}
	}
}
