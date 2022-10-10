using System;

namespace AlgoriaCore.Application.Managers.Users.Dto
{
    public class UserPasswordHistoryDto
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string Password { get; set; }
        public DateTime? Date { get; set; }
    }
}
