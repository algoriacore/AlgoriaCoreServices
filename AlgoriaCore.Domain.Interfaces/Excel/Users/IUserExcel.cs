namespace AlgoriaCore.Domain.Interfaces.Excel.Users
{
    public interface IUserExcel
    {
        long Id { get; set; }
        string Login { get; set; }
        string Name { get; set; } 
        string LastName { get; set; }
        string SecondLastName { get; set; }
        string FullName { get; set; }
        string EmailAddress { get; set; }
        string PhoneNumber { get; set; }
        string UserLockedDesc { get; set; }
        string IsActiveDesc { get; set; }
    }
}
