using AlgoriaCore.Domain.Entities.Base;
using System;

namespace AlgoriaCore.Domain.Entities
{
    public partial class UserReset : Entity<long>
    {
        public long? UserId { get; set; }
        public string ResetCode { get; set; }
        public DateTime? Validity { get; set; }

        public virtual User User { get; set; }
    }
}
