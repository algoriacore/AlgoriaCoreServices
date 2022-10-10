using AlgoriaCore.Domain.Interfaces.Email;

namespace AlgoriaCore.Domain.Email
{
    public class EmailAddress : IEmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
