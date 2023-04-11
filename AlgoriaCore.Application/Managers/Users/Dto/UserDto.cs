using AlgoriaCore.Extensions;
using System;
using System.Text;

namespace AlgoriaCore.Application.Managers.Users.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        public int? TenantId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }

        private string fullName;

		public string FullName
        {
            get
            {
                if (!fullName.IsNullOrEmpty())
                {
                    return fullName;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    if (!Name.IsNullOrEmpty())
                    {
                        sb.Append(Name);
                    }

                    if (!LastName.IsNullOrEmpty())
                    {
                        sb.AppendFormat(" {0}", LastName);
                    }

                    if (!SecondLastName.IsNullOrEmpty())
                    {
                        sb.AppendFormat(" {0}", SecondLastName);
                    }

                    fullName = sb.ToString();
                    return fullName;
                }
            }
            set
            {
                fullName = value;
            }
        }

        public string EmailAddress { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsPhoneNumberConfirmed { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool? ChangePassword { get; set; }
        public int AccesFailedCount { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public Guid? ProfilePictureId { get; set; }
        public bool? UserLocked { get; set; }
        public string UserLockedDesc { get; set; }
        public bool? IsLockoutEnabled { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
        public bool IsDeleted { get; set; }
        public string TenancyName { get; set; }
    }
}
