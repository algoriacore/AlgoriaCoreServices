namespace AlgoriaCore.Application.Managers.Permissions.Dto
{
    public class PermissionGetIsGrantedDto
    {
        public bool RequiresAll { get; set; }
        public string[] PermissionNames { get; set; }
    }
}
