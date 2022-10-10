using System.Collections.Generic;

namespace AlgoriaCore.Domain.Interfaces.Authorization
{
    public interface IAppPermission
    {
        string Name { get; set; }
        string Description { get; set; }
        AppPermissionScope PermissionScope { get; set; }
        IAppPermission Parent { get; set; }
        bool HasChildren { get; }

        List<IAppPermission> Children { get; }
    }


    public enum AppPermissionScope
    {
        Host,
        Tenant,
        Both
    }
}
