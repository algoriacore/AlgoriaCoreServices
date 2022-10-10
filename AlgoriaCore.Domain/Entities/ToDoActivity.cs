using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class ToDoActivity : Entity<long>, IMayHaveTenant
    {
        public ToDoActivity()
        {
            ToDoActEvaluator = new HashSet<ToDoActEvaluator>();
            ToDoActExecutor = new HashSet<ToDoActExecutor>();
            ToDoTimeSheet = new HashSet<ToDoTimeSheet>();
        }

        public int? TenantId { get; set; }
        public long UserCreator { get; set; }
        public long Status { get; set; }
        public DateTime? CreationTime { get; set; }
        public string? Description { get; set; }
        public DateTime? InitialPlannedDate { get; set; }
        public DateTime? InitialRealDate { get; set; }
        public DateTime? FinalPlannedDate { get; set; }
        public DateTime? FinalRealDate { get; set; }
        public bool? IsOnTime { get; set; }
        public string? table { get; set; }
        public long? key { get; set; }

        public virtual TemplateToDoStatus StatusNavigation { get; set; } = null!;
        public virtual Tenant? Tenant { get; set; }
        public virtual User UserCreatorNavigation { get; set; } = null!;
        public virtual ICollection<ToDoActEvaluator> ToDoActEvaluator { get; set; }
        public virtual ICollection<ToDoActExecutor> ToDoActExecutor { get; set; }
        public virtual ICollection<ToDoTimeSheet> ToDoTimeSheet { get; set; }
    }
}
