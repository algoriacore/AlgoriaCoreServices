namespace AlgoriaCore.Application.Managers.Roles.Dto
{
    public class PermissionDto
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsGranted { get; set; }
    }
}
