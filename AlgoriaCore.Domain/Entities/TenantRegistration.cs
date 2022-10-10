using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class TenantRegistration : Entity<int>
    {
        public string TenancyName { get; set; }
        public string TenantName { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string SecondLastname { get; set; }
        public string EmailAddress { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
