namespace AlgoriaCore.Domain.Interfaces.Authorization
{
    public interface IPermissionProvider
    {
        IAppPermission GetPermissionsTree(AppPermissionScope scope);
    }
}
