using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class UserARN : Entity<long>
    {
        public long? UserId { get; set; }
        public string? ARNCode { get; set; }

        public virtual User? User { get; set; }
    }
}
