using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands
{
    public class TenantCreateCommand : IRequest<int>
    {
        public string TenancyName { get; set; }
        public string TenantName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string EmailAddress { get; set; }
    }
}
