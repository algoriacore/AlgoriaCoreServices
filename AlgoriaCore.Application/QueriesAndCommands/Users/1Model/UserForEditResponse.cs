using AlgoriaCore.Application.BaseClases.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._1Model
{
    public class UserForEditResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? SetRandomPassword { get; set; }
        public bool? ShouldChangePasswordOnNextLogin { get; set; }
        public bool? SendActivationEmail { get; set; }
        public int? Language { get; set; }
        public Dictionary<string, string> Preferences { get; set; }
        public List<ComboboxItemDto> LanguageCombo { get; set; }

        public List<UserRolResponse> RolList { get; set; }

        public UserForEditResponse()
        {
            Preferences = new Dictionary<string, string>();
            RolList = new List<UserRolResponse>();
            LanguageCombo = new List<ComboboxItemDto>();
        }
    }
}
