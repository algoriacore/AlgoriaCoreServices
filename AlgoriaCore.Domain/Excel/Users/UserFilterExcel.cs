using AlgoriaCore.Domain.Interfaces.Excel.Users;

namespace AlgoriaCore.Domain.Excel.Users
{
    public class UserFilterExcel : IUserFilterExcel
    {
        public string Filter { get; set; }
    }
}
