namespace AlgoriaCore.Application.Managers.Tenants.Dto
{
    public class TenantRegistrationDto
    {
        public int Id { get; set; }
        public string TenancyName { get; set; }
        public string TenantName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string EmailAddress { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
