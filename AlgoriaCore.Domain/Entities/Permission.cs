
using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class Permission : Entity<long>
    {
        public long? Role { get; set; }
        public string Name { get; set; }
        public bool IsGranted { get; set; }

        public virtual Role RoleNavigation { get; set; }
    }
}
