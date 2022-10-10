using System;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants
{
    public class TenantResponse
    {
        public int Id { get; set; }
        public string TenancyName { get; set; }
        public string Name { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
