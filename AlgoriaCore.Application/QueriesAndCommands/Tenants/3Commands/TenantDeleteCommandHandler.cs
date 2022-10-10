using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Tenants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants._3Commands
{
    public class TenantDeleteCommandHandler : BaseCoreClass, IRequestHandler<TenantDeleteCommand, int>
    {
        private readonly TenantManager _manager;

        public TenantDeleteCommandHandler(ICoreServices coreServices, TenantManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<int> Handle(TenantDeleteCommand request, CancellationToken cancellationToken)
        {
            return await _manager.DeleteTenantAsync(request.Id);
        }
    }
}
