using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Application.Managers.Tenants.Dto;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands
{
    public class TenantCreateRegistrationCommandHandler : BaseCoreClass, IRequestHandler<TenantCreateRegistrationCommand, string>
	{
		private readonly TenantRegistrationManager _tenantManager;

		public TenantCreateRegistrationCommandHandler(TenantRegistrationManager tenantManager,
													ILogger<TenantCreateRegistrationCommandHandler> logger,
													ICoreServices coreServices)
								: base(coreServices)
		{
			_tenantManager = tenantManager;
		}

		public async Task<string> Handle(TenantCreateRegistrationCommand request, CancellationToken cancellationToken)
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
				var resp = await _tenantManager.CreateTenantRegistrationAsync(tReg);
				return resp;
			}
		}
	}
}
