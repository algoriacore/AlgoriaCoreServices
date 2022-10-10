using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Tenants;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands
{
    public class TenantConfirmRegistrationCommandHandler : BaseCoreClass, IRequestHandler<TenantConfirmRegistrationCommand, int>
	{
		private readonly TenantRegistrationManager _tenantManager;

		public TenantConfirmRegistrationCommandHandler(TenantRegistrationManager tenantManager,
													ILogger<TenantConfirmRegistrationCommandHandler> logger,
													ICoreServices coreServices)
								: base(coreServices)
		{
			_tenantManager = tenantManager;
		}

		public async Task<int> Handle(TenantConfirmRegistrationCommand request, CancellationToken cancellationToken)
		{
			using (_tenantManager.CurrentUnitOfWork.SetTenantId(null))
			{
				var resp = await _tenantManager.ConfirmTenantRegistrationAsync(request.Code);
				return resp;
			}
		}
	}
}
