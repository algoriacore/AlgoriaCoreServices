using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserUpdateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? SetRandomPassword { get; set; }
        public bool? ShouldChangePasswordOnNextLogin { get; set; }

        public List<string> AssignedRoleNames { get; set; }
    }
}
