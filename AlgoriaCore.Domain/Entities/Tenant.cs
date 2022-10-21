using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
#nullable enable
    public partial class Tenant : Entity<int>, ISoftDelete
    {
        public Tenant()
        {
            AuditLog = new HashSet<AuditLog>();
            BinaryObjects = new HashSet<BinaryObjects>();
            ChangeLog = new HashSet<ChangeLog>();
            ChatMessageFriendTenant = new HashSet<ChatMessage>();
            ChatMessageTenant = new HashSet<ChatMessage>();
            ChatRoom = new HashSet<ChatRoom>();
            ChatRoomChat = new HashSet<ChatRoomChat>();
            FriendshipFriendTenant = new HashSet<Friendship>();
            FriendshipTenant = new HashSet<Friendship>();
            Language = new HashSet<Language>();
            MailServiceRequest = new HashSet<MailServiceRequest>();
            OrgUnit = new HashSet<OrgUnit>();
            Role = new HashSet<Role>();
            SampleDateData = new HashSet<SampleDateData>();
            Setting = new HashSet<Setting>();
            SettingClient = new HashSet<SettingClient>();
            User = new HashSet<User>();
            help = new HashSet<help>();
            mailgroup = new HashSet<mailgroup>();
            mailtemplate = new HashSet<mailtemplate>();
        }

        public string? TenancyName { get; set; }
        public string? Name { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<AuditLog> AuditLog { get; set; }
        public virtual ICollection<BinaryObjects> BinaryObjects { get; set; }
        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageFriendTenant { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageTenant { get; set; }
        public virtual ICollection<ChatRoom> ChatRoom { get; set; }
        public virtual ICollection<ChatRoomChat> ChatRoomChat { get; set; }
        public virtual ICollection<Friendship> FriendshipFriendTenant { get; set; }
        public virtual ICollection<Friendship> FriendshipTenant { get; set; }
        public virtual ICollection<Language> Language { get; set; }
        public virtual ICollection<MailServiceRequest> MailServiceRequest { get; set; }
        public virtual ICollection<OrgUnit> OrgUnit { get; set; }
        public virtual ICollection<Role> Role { get; set; }
        public virtual ICollection<SampleDateData> SampleDateData { get; set; }
        public virtual ICollection<Setting> Setting { get; set; }
        public virtual ICollection<SettingClient> SettingClient { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<help> help { get; set; }
        public virtual ICollection<mailgroup> mailgroup { get; set; }
        public virtual ICollection<mailtemplate> mailtemplate { get; set; }
    }
}
