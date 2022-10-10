using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Application.Managers.Tenants.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands
{
    public class TenantCreateCommandHandler : BaseCoreClass, IRequestHandler<TenantCreateCommand, int>
	{
		private readonly TenantRegistrationManager _tenantManager;

		public TenantCreateCommandHandler(
			TenantRegistrationManager tenantManager
			, ICoreServices coreServices) : base(coreServices)
		{
			_tenantManager = tenantManager;
		}

		public async Task<int> Handle(TenantCreateCommand request, CancellationToken cancellationToken)
		{
			var tReg = new TenantRegistrationDto();
			tReg.TenancyName = request.TenancyName;
			tReg.TenantName = request.TenantName;
			tReg.Password = request.Password;
			tReg.Name = request.Name;
			tReg.LastName = request.LastName;
			tReg.SecondLastName = request.SecondLastName;
			tReg.EmailAddress = request.EmailAddress;

			using (_tenantManager.CurrentUnitOfWork.SetTenantId(null))
			{
				var confirmationCode = await _tenantManager.CreateTenantRegistrationAsync(tReg, false);
				var resp = await _tenantManager.ConfirmTenantRegistrationAsync(confirmationCode);

				return resp;
			}
		}
	}
}
