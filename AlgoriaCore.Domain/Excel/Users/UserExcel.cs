using AlgoriaCore.Domain.Interfaces.Excel.Users;
using System;

namespace AlgoriaCore.Domain.Excel.Users
{
    public class UserExcel : IUserExcel
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string UserLockedDesc { get; set; }
        public string IsActiveDesc { get; set; }
    }
}
