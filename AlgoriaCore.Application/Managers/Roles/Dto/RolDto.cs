namespace AlgoriaCore.Application.Managers.Roles.Dto
{
    public class RolDto
    {
        public long? Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }      
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
