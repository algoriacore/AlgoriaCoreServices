using AlgoriaCore.Domain.Entities.Base;
using System;

namespace AlgoriaCore.Domain.Entities
{
    public partial class UserPasswordHistory : Entity<long>
    {
        public long? UserId { get; set; }
        public string Password { get; set; }
        public DateTime? Date { get; set; }

        public virtual User User { get; set; }
    }
}
