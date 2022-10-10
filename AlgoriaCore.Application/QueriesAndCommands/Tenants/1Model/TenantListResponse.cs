using System;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants._1Model
{
    public class TenantListResponse
    {
        public int Id { get; set; }
        public string TenancyName { get; set; }
        public string Name { get; set; }
        public string LargeName { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
    }
}
