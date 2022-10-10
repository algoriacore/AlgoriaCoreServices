using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;

namespace AlgoriaCore.Domain.Entities
{
    public partial class ToDoTimeSheet : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long ToDoActivity { get; set; }
        public long UserCreator { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Comments { get; set; }
        public decimal? HoursSpend { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual ToDoActivity ToDoActivityNavigation { get; set; }
        public virtual User UserCreatorNavigation { get; set; }
    }
}
