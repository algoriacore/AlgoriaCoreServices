using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Domain.Authorization;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Authorization.Permissions
{
    public class PermissionGetTreeQueryHandler : BaseCoreClass, IRequestHandler<PermissionGetTreeQuery, Permission>
    {
        private readonly IAppAuthorizationProvider _authorizationProvider;

        public PermissionGetTreeQueryHandler(
            ICoreServices coreServices,
            IAppAuthorizationProvider authorizationProvider) : base(coreServices)
        {
            _authorizationProvider = authorizationProvider;
        }

        public async Task<Permission> Handle(PermissionGetTreeQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_authorizationProvider.GetPermissions(SessionContext));
        }
    }
}
