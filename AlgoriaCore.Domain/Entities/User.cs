using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class User : Entity<long>, IMayHaveTenant, ISoftDelete
    {
        public User()
        {
            AuditLogImpersonalizerUser = new HashSet<AuditLog>();
            AuditLogUser = new HashSet<AuditLog>();
            ChangeLog = new HashSet<ChangeLog>();
            ChatMessageFriendUser = new HashSet<ChatMessage>();
            ChatMessageUser = new HashSet<ChatMessage>();
            ChatRoom = new HashSet<ChatRoom>();
            ChatRoomChat = new HashSet<ChatRoomChat>();
            ChatRoomChatUserTagged = new HashSet<ChatRoomChatUserTagged>();
            FriendshipFriendUser = new HashSet<Friendship>();
            FriendshipUser = new HashSet<Friendship>();
            MailServiceRequest = new HashSet<MailServiceRequest>();
            OrgUnitUser = new HashSet<OrgUnitUser>();
            Setting = new HashSet<Setting>();
            SettingClient = new HashSet<SettingClient>();
            UserPasswordHistory = new HashSet<UserPasswordHistory>();
            UserRole = new HashSet<UserRole>();
        }

        public int? TenantId { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string SecondLastname { get; set; }
        public string EmailAddress { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsPhoneConfirmed { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool? ChangePassword { get; set; }
        public int? AccessFailedCount { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public Guid? ProfilePictureId { get; set; }
        public bool? UserLocked { get; set; }
        public bool? IsLockoutEnabled { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual UserARN UserARN { get; set; }
        public virtual UserReset UserReset { get; set; }
        public virtual ICollection<AuditLog> AuditLogImpersonalizerUser { get; set; }
        public virtual ICollection<AuditLog> AuditLogUser { get; set; }
        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageFriendUser { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageUser { get; set; }
        public virtual ICollection<ChatRoom> ChatRoom { get; set; }
        public virtual ICollection<ChatRoomChat> ChatRoomChat { get; set; }
        public virtual ICollection<ChatRoomChatUserTagged> ChatRoomChatUserTagged { get; set; }
        public virtual ICollection<Friendship> FriendshipFriendUser { get; set; }
        public virtual ICollection<Friendship> FriendshipUser { get; set; }
        public virtual ICollection<MailServiceRequest> MailServiceRequest { get; set; }
        public virtual ICollection<OrgUnitUser> OrgUnitUser { get; set; }
        public virtual ICollection<Setting> Setting { get; set; }
        public virtual ICollection<SettingClient> SettingClient { get; set; }
        public virtual ICollection<UserPasswordHistory> UserPasswordHistory { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
