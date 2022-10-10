using System;

namespace AlgoriaCore.Application.Managers.Tenants.Dto
{
    public class TenantDto
    {
        public int Id { get; set; }
        public string TenancyName { get; set; }
        public string Name { get; set; }
        public string LargeName { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
