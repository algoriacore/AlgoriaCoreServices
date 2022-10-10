namespace AlgoriaCore.Application.QueriesAndCommands.Roles._1Model
{
    public class RolResponse
    {
        public long Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
