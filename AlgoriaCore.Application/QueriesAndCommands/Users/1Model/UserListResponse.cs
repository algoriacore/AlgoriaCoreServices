using System;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._1Model
{
    public class UserListResponse
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string EmailAddress { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public bool? UserLocked { get; set; }
        public string UserLockedDesc { get; set; }
        public bool? IsActive { get; set; }
        public string IsActiveDesc { get; set; }
        public DateTime? CreationTime { get; set; }
        public string FullName { get; set; }
    }
}
