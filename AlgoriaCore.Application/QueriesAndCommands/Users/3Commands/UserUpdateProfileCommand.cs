using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserUpdateProfileCommand : IRequest<long>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public string PictureName { get; set; }

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }

        public int? Language { get; set; }
        public string ClientType { get; set; }
        public Dictionary<string, string> Preferences { get; set; }
    }
}
