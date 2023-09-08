using AlgoriaCore.Domain.MultiTenancy;
using AlgoriaCore.Domain.Session;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Authorization
{
    public interface IAppAuthorizationProvider
    {
        Permission Root { get; }

        Permission GetPermissions(IAppSession session);

        List<Permission> GetPermissionsFromNamesByValidating(List<string> permissionNames);

        Permission GetPermissionByName(string name);
        List<string> GetPermissionNamesList(MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant);
    }
}
