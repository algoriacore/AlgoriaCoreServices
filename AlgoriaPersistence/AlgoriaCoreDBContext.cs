using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.SqlFunctions;
using Microsoft.EntityFrameworkCore;
using System;

namespace AlgoriaPersistence
{
    public partial class AlgoriaCoreDbContext : DbContext
    {
        public AlgoriaCoreDbContext(DbContextOptions<AlgoriaCoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditLog> AuditLog { get; set; } = null!;
        public virtual DbSet<BinaryObjects> BinaryObjects { get; set; } = null!;
        public virtual DbSet<ChangeLog> ChangeLog { get; set; } = null!;
        public virtual DbSet<ChangeLogDetail> ChangeLogDetail { get; set; } = null!;
        public virtual DbSet<ChatMessage> ChatMessage { get; set; } = null!;
        public virtual DbSet<ChatRoom> ChatRoom { get; set; } = null!;
        public virtual DbSet<ChatRoomChat> ChatRoomChat { get; set; } = null!;
        public virtual DbSet<ChatRoomChatFile> ChatRoomChatFile { get; set; } = null!;
        public virtual DbSet<ChatRoomChatUserTagged> ChatRoomChatUserTagged { get; set; } = null!;
        public virtual DbSet<Friendship> Friendship { get; set; } = null!;
        public virtual DbSet<Language> Language { get; set; } = null!;
        public virtual DbSet<LanguageText> LanguageText { get; set; } = null!;
        public virtual DbSet<MailServiceMail> MailServiceMail { get; set; } = null!;
        public virtual DbSet<MailServiceMailAttach> MailServiceMailAttach { get; set; } = null!;
        public virtual DbSet<MailServiceMailBody> MailServiceMailBody { get; set; } = null!;
        public virtual DbSet<MailServiceMailConfig> MailServiceMailConfig { get; set; } = null!;
        public virtual DbSet<MailServiceMailStatus> MailServiceMailStatus { get; set; } = null!;
        public virtual DbSet<MailServiceRequest> MailServiceRequest { get; set; } = null!;
        public virtual DbSet<OUUsersSecurity> OUUsersSecurity { get; set; } = null!;
        public virtual DbSet<OrgUnit> OrgUnit { get; set; } = null!;
        public virtual DbSet<OrgUnitUser> OrgUnitUser { get; set; } = null!;
        public virtual DbSet<Permission> Permission { get; set; } = null!;
        public virtual DbSet<Role> Role { get; set; } = null!;
        public virtual DbSet<SampleDateData> SampleDateData { get; set; } = null!;
        public virtual DbSet<Setting> Setting { get; set; } = null!;
        public virtual DbSet<SettingClient> SettingClient { get; set; } = null!;
        public virtual DbSet<Template> Template { get; set; } = null!;
        public virtual DbSet<TemplateDefaultOUEditor> TemplateDefaultOUEditor { get; set; } = null!;
        public virtual DbSet<TemplateDefaultOUReader> TemplateDefaultOUReader { get; set; } = null!;
        public virtual DbSet<TemplateDefaultUserEditor> TemplateDefaultUserEditor { get; set; } = null!;
        public virtual DbSet<TemplateDefaultUserReader> TemplateDefaultUserReader { get; set; } = null!;
        public virtual DbSet<TemplateField> TemplateField { get; set; } = null!;
        public virtual DbSet<TemplateFieldOption> TemplateFieldOption { get; set; } = null!;
        public virtual DbSet<TemplateFieldRelation> TemplateFieldRelation { get; set; } = null!;
        public virtual DbSet<TemplateQuery> TemplateQuery { get; set; } = null!;
        public virtual DbSet<TemplateSection> TemplateSection { get; set; } = null!;
        public virtual DbSet<TemplateToDoStatus> TemplateToDoStatus { get; set; } = null!;
        public virtual DbSet<Tenant> Tenant { get; set; } = null!;
        public virtual DbSet<TenantRegistration> TenantRegistration { get; set; } = null!;
        public virtual DbSet<ToDoActEvaluator> ToDoActEvaluator { get; set; } = null!;
        public virtual DbSet<ToDoActExecutor> ToDoActExecutor { get; set; } = null!;
        public virtual DbSet<ToDoActivity> ToDoActivity { get; set; } = null!;
        public virtual DbSet<ToDoTimeSheet> ToDoTimeSheet { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<UserARN> UserARN { get; set; } = null!;
        public virtual DbSet<UserPasswordHistory> UserPasswordHistory { get; set; } = null!;
        public virtual DbSet<UserReset> UserReset { get; set; } = null!;
        public virtual DbSet<UserRole> UserRole { get; set; } = null!;
        public virtual DbSet<help> help { get; set; } = null!;
        public virtual DbSet<helptxt> helptxt { get; set; } = null!;
        public virtual DbSet<mailgroup> mailgroup { get; set; } = null!;
        public virtual DbSet<mailgrouptxt> mailgrouptxt { get; set; } = null!;
        public virtual DbSet<mailtemplate> mailtemplate { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasIndex(e => new { e.TenantId, e.ExecutionDatetime }, "IX_AuditLog_ExecutionDatetime");

                entity.HasIndex(e => new { e.TenantId, e.ExecutionDuration }, "IX_AuditLog_ExecutionDuration");

                entity.HasIndex(e => new { e.TenantId, e.MethodName }, "IX_AuditLog_MethodName");

                entity.HasIndex(e => new { e.TenantId, e.ServiceName }, "IX_AuditLog_ServiceName");

                entity.HasIndex(e => new { e.TenantId, e.Severity }, "IX_AuditLog_Severity");

                entity.Property(e => e.BroserInfo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ClientIpAddress)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.ClientName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Exception).IsUnicode(false);

                entity.Property(e => e.ExecutionDatetime).HasColumnType("datetime");

                entity.Property(e => e.MethodName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.ImpersonalizerUser)
                    .WithMany(p => p.AuditLogImpersonalizerUser)
                    .HasForeignKey(d => d.ImpersonalizerUserId)
                    .HasConstraintName("FK_AuditLog_ImpersonalizerUserId");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.AuditLog)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_AuditLog_Tenant");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuditLogUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AuditLog_User");
            });

            modelBuilder.Entity<BinaryObjects>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FileName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.BinaryObjects)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_BinaryObjects_Tenant");
            });

            modelBuilder.Entity<ChangeLog>(entity =>
            {
                entity.HasIndex(e => e.table, "IX_ChangeLog_Table");

                entity.HasIndex(e => new { e.table, e.key }, "IX_ChangeLog_TableKey");

                entity.Property(e => e.datetime).HasColumnType("datetime");

                entity.Property(e => e.key)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.table)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ChangeLog)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_ChangeLog_Tenant");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ChangeLog)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ChangeLog_User");
            });

            modelBuilder.Entity<ChangeLogDetail>(entity =>
            {
                entity.Property(e => e.data).IsUnicode(false);

                entity.HasOne(d => d.changelogNavigation)
                    .WithMany(p => p.ChangeLogDetail)
                    .HasForeignKey(d => d.changelog)
                    .HasConstraintName("FK_ChangeLogDetail_ChangeLog");
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasIndex(e => e.CreationTime, "IX_ChatMessage_CreationTime");

                entity.HasIndex(e => new { e.TenantId, e.UserId }, "IX_ChatMessage_UserId");

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.Message).IsUnicode(false);

                entity.HasOne(d => d.FriendTenant)
                    .WithMany(p => p.ChatMessageFriendTenant)
                    .HasForeignKey(d => d.FriendTenantId)
                    .HasConstraintName("FK_ChatMessage_FriendTenantId");

                entity.HasOne(d => d.FriendUser)
                    .WithMany(p => p.ChatMessageFriendUser)
                    .HasForeignKey(d => d.FriendUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessage_FriendUserId");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ChatMessageTenant)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_ChatMessage_TenantId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ChatMessageUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessage_UserId");
            });

            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.HasIndex(e => new { e.TenantId, e.ChatRoomId }, "IX_ChatRoom_ChatRoomId");

                entity.HasIndex(e => new { e.TenantId, e.CreationTime }, "IX_ChatRoom_CreationTime");

                entity.HasIndex(e => new { e.TenantId, e.Description }, "IX_ChatRoom_Description");

                entity.HasIndex(e => new { e.TenantId, e.Name }, "IX_ChatRoom_Name");

                entity.HasIndex(e => new { e.TenantId, e.ChatRoomId }, "UQ_ChatRoom_ChatRoomId")
                    .IsUnique();

                entity.Property(e => e.ChatRoomId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ChatRoom)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_ChatRoom_Tenant");

                entity.HasOne(d => d.UserCreatorNavigation)
                    .WithMany(p => p.ChatRoom)
                    .HasForeignKey(d => d.UserCreator)
                    .HasConstraintName("FK_ChatRoom_User");
            });

            modelBuilder.Entity<ChatRoomChat>(entity =>
            {
                entity.HasIndex(e => new { e.TenantId, e.CreationTime }, "IX_ChatRoomChat_CreationTime");

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.HasOne(d => d.ChatRoomNavigation)
                    .WithMany(p => p.ChatRoomChat)
                    .HasForeignKey(d => d.ChatRoom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatRoomChat_ChatRoom");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ChatRoomChat)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_ChatRoomChat_Tenant");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.ChatRoomChat)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FK_ChatRoomChat_User");
            });

            modelBuilder.Entity<ChatRoomChatFile>(entity =>
            {
                entity.Property(e => e.FileExtension)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.ChatRoomChatNavigation)
                    .WithMany(p => p.ChatRoomChatFile)
                    .HasForeignKey(d => d.ChatRoomChat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatRoomChatFile_ChatRoomChat");
            });

            modelBuilder.Entity<ChatRoomChatUserTagged>(entity =>
            {
                entity.HasOne(d => d.ChatRoomChatNavigation)
                    .WithMany(p => p.ChatRoomChatUserTagged)
                    .HasForeignKey(d => d.ChatRoomChat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatRoomChatUserTagged_ChatRoomChat");

                entity.HasOne(d => d.UserTaggedNavigation)
                    .WithMany(p => p.ChatRoomChatUserTagged)
                    .HasForeignKey(d => d.UserTagged)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatRoomChatUserTagged_User");
            });

            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.HasIndex(e => e.CreationTime, "IX_Friendship_CreationTime");

                entity.HasIndex(e => new { e.TenantId, e.UserId }, "IX_Friendship_UserId");

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.FriendNickname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.FriendTenant)
                    .WithMany(p => p.FriendshipFriendTenant)
                    .HasForeignKey(d => d.FriendTenantId)
                    .HasConstraintName("FK_Friendship_FriendTenantId");

                entity.HasOne(d => d.FriendUser)
                    .WithMany(p => p.FriendshipFriendUser)
                    .HasForeignKey(d => d.FriendUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friendship_FriendUserId");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.FriendshipTenant)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_Friendship_TenantId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FriendshipUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friendship_UserId");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.Language)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_Language_Tenant");
            });

            modelBuilder.Entity<LanguageText>(entity =>
            {
                entity.HasIndex(e => new { e.LanguageId, e.Key }, "UQ_LanguageText_Key")
                    .IsUnique();

                entity.Property(e => e.Key)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Value).IsUnicode(false);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.LanguageText)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_LanguageText_Language");
            });

            modelBuilder.Entity<MailServiceMail>(entity =>
            {
                entity.Property(e => e.BlindCopyTo)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CopyTo)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Sendto)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.MailServiceRequestNavigation)
                    .WithMany(p => p.MailServiceMail)
                    .HasPrincipalKey(p => new { p.TenantId, p.Id })
                    .HasForeignKey(d => new { d.TenantId, d.MailServiceRequest })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MailServiceMail_MailServiceRequest");
            });

            modelBuilder.Entity<MailServiceMailAttach>(entity =>
            {
                entity.Property(e => e.ContenType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.MailServiceMailBodyNavigation)
                    .WithMany(p => p.MailServiceMailAttach)
                    .HasForeignKey(d => d.MailServiceMailBody)
                    .HasConstraintName("FK_MailServiceMailAttach_MailServiceMailBody");
            });

            modelBuilder.Entity<MailServiceMailBody>(entity =>
            {
                entity.HasIndex(e => e.MailServiceMail, "UQ_MailServiceMailBody_MailServiceMail")
                    .IsUnique();

                entity.Property(e => e.Body).IsUnicode(false);

                entity.HasOne(d => d.MailServiceMailNavigation)
                    .WithOne(p => p.MailServiceMailBody)
                    .HasForeignKey<MailServiceMailBody>(d => d.MailServiceMail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MailServiceMailBody_MailServiceMail");
            });

            modelBuilder.Entity<MailServiceMailConfig>(entity =>
            {
                entity.HasIndex(e => e.MailServiceMail, "UQ_MailServiceMailConfig_MailServiceMail")
                    .IsUnique();

                entity.Property(e => e.Domain)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MailPassword)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MailUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SMPTHost)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Sender)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SenderDisplay)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.MailServiceMailNavigation)
                    .WithOne(p => p.MailServiceMailConfig)
                    .HasForeignKey<MailServiceMailConfig>(d => d.MailServiceMail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MailServiceMailConfig_MailServiceMail");
            });

            modelBuilder.Entity<MailServiceMailStatus>(entity =>
            {
                entity.HasIndex(e => e.MailServiceMail, "UQ_MailServiceMailStatus_MailServiceMail")
                    .IsUnique();

                entity.Property(e => e.Error).IsUnicode(false);

                entity.Property(e => e.SentTime).HasColumnType("datetime");

                entity.HasOne(d => d.MailServiceMailNavigation)
                    .WithOne(p => p.MailServiceMailStatus)
                    .HasForeignKey<MailServiceMailStatus>(d => d.MailServiceMail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MailServiceMailStatus_MailServiceMail");
            });

            modelBuilder.Entity<MailServiceRequest>(entity =>
            {
                entity.HasIndex(e => new { e.TenantId, e.Id }, "UQ_MailServiceRequest_TenantId")
                    .IsUnique();

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.MailServiceRequest)
                    .HasForeignKey(d => d.TenantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MailServiceRequest_Tenant");

                entity.HasOne(d => d.UserCreatorNavigation)
                    .WithMany(p => p.MailServiceRequest)
                    .HasForeignKey(d => d.UserCreator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MailServiceRequest_User");
            });

            modelBuilder.Entity<OUUsersSecurity>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("OUUsersSecurity");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrgUnit>(entity =>
            {
                entity.HasIndex(e => new { e.TenantId, e.Name }, "IX_OrgUnit_Name");
                entity.HasIndex(e => e.IsDeleted, "IX_OrgUnit_IsDeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParentOUNavigation)
                    .WithMany(p => p.InverseParentOUNavigation)
                    .HasForeignKey(d => d.ParentOU)
                    .HasConstraintName("FK_OrgUnit_OrgUnit");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.OrgUnit)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_OrgUnit_Tenant");
            });

            modelBuilder.Entity<OrgUnitUser>(entity =>
            {
                entity.HasOne(d => d.OrgUnitNavigation)
                    .WithMany(p => p.OrgUnitUser)
                    .HasForeignKey(d => d.OrgUnit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrgUnitUser_OrgUnit");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.OrgUnitUser)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrgUnitUser_User");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_Permission_Name");

                entity.HasIndex(e => new { e.Role, e.Name }, "UQ_Permission_RolePermission")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK_Permission_Role");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.Role)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_Role_Tenant");
            });

            modelBuilder.Entity<SampleDateData>(entity =>
            {
                entity.Property(e => e.DateData).HasColumnType("date");

                entity.Property(e => e.DateTimeData).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.SampleDateData)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_SampleDateData_Tenant");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_Setting_Name");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.value)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.Setting)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_Setting_Tenant");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Setting)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Setting_User");
            });

            modelBuilder.Entity<SettingClient>(entity =>
            {
                entity.HasIndex(e => new { e.ClientType, e.Name }, "IX_SettingClient_ClientType");

                entity.Property(e => e.ClientType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.value)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.SettingClient)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_SettingClient_Tenant");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SettingClient)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SettingClient_User");
            });

            modelBuilder.Entity<Template>(entity =>
            {
                entity.HasIndex(e => e.TableName, "UQ_Template_TableName")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.NamePlural)
                    .HasMaxLength(22)
                    .IsUnicode(false);

                entity.Property(e => e.NameSingular)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RGBColor)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.Template)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_Template_Tenant");
            });

            modelBuilder.Entity<TemplateDefaultOUEditor>(entity =>
            {
                entity.HasIndex(e => e.IsExecutor, "IX_TemplateDefaultOUEditor_IsExecutor");

                entity.HasOne(d => d.OrgUnitNavigation)
                    .WithMany(p => p.TemplateDefaultOUEditor)
                    .HasForeignKey(d => d.OrgUnit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateDefaultOUEditor_OrgUnit");

                entity.HasOne(d => d.TemplateNavigation)
                    .WithMany(p => p.TemplateDefaultOUEditor)
                    .HasForeignKey(d => d.Template)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateDefaultOUEditor_Template");
            });

            modelBuilder.Entity<TemplateDefaultOUReader>(entity =>
            {
                entity.HasOne(d => d.OrgUnitNavigation)
                    .WithMany(p => p.TemplateDefaultOUReader)
                    .HasForeignKey(d => d.OrgUnit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateDefaultOUReader_OrgUnit");

                entity.HasOne(d => d.TemplateNavigation)
                    .WithMany(p => p.TemplateDefaultOUReader)
                    .HasForeignKey(d => d.Template)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateDefaultOUReader_Template");
            });

            modelBuilder.Entity<TemplateDefaultUserEditor>(entity =>
            {
                entity.HasIndex(e => e.IsExecutor, "IX_TemplateDefaultUserEditor_IsExecutor");

                entity.HasOne(d => d.TemplateNavigation)
                    .WithMany(p => p.TemplateDefaultUserEditor)
                    .HasForeignKey(d => d.Template)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateDefaultUserEditor_Template");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.TemplateDefaultUserEditor)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateDefaultUserEditor_User");
            });

            modelBuilder.Entity<TemplateDefaultUserReader>(entity =>
            {
                entity.HasOne(d => d.TemplateNavigation)
                    .WithMany(p => p.TemplateDefaultUserReader)
                    .HasForeignKey(d => d.Template)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateDefaultUserReader_Template");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.TemplateDefaultUserReader)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateDefaultUserReader_User");
            });

            modelBuilder.Entity<TemplateField>(entity =>
            {
                entity.Property(e => e.FieldName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.InputMask)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KeyFilter)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.TemplateSectionNavigation)
                    .WithMany(p => p.TemplateField)
                    .HasForeignKey(d => d.TemplateSection)
                    .HasConstraintName("FK_TemplateField_TemplateSection");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.TemplateField)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_TemplateField_Tenant");
            });

            modelBuilder.Entity<TemplateFieldOption>(entity =>
            {
                entity.HasIndex(e => new { e.TemplateField, e.value }, "UQ_TemplateFieldOption_value")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.TemplateFieldNavigation)
                    .WithMany(p => p.TemplateFieldOption)
                    .HasForeignKey(d => d.TemplateField)
                    .HasConstraintName("FK_TemplateFieldOption_TemplateField");
            });

            modelBuilder.Entity<TemplateFieldRelation>(entity =>
            {
                entity.HasIndex(e => e.TemplateField, "UQ_TemplateFieldRelation_TemplateField")
                    .IsUnique();

                entity.Property(e => e.TemplateFieldRelation1).HasColumnName("TemplateFieldRelation");

                entity.HasOne(d => d.TemplateFieldNavigation)
                    .WithOne(p => p.TemplateFieldRelationTemplateFieldNavigation)
                    .HasForeignKey<TemplateFieldRelation>(d => d.TemplateField)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateFieldRelation_TemplateField");

                entity.HasOne(d => d.TemplateFieldRelation1Navigation)
                    .WithMany(p => p.TemplateFieldRelationTemplateFieldRelation1Navigation)
                    .HasForeignKey(d => d.TemplateFieldRelation1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateFieldRelation_TemplateFieldRelation");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.TemplateFieldRelation)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_TemplateFieldRelation_Tenant");
            });

            modelBuilder.Entity<TemplateQuery>(entity =>
            {
                entity.HasIndex(e => new { e.Template, e.QueryType }, "UQ_TemplateQuery_QueryType")
                    .IsUnique();

                entity.Property(e => e.Query).IsUnicode(false);

                entity.HasOne(d => d.TemplateNavigation)
                    .WithMany(p => p.TemplateQuery)
                    .HasForeignKey(d => d.Template)
                    .HasConstraintName("FK_TemplateQuery_Template");
            });

            modelBuilder.Entity<TemplateSection>(entity =>
            {
                entity.Property(e => e.IconAF)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.TemplateNavigation)
                    .WithMany(p => p.TemplateSection)
                    .HasForeignKey(d => d.Template)
                    .HasConstraintName("FK_TemplateSection_Template");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.TemplateSection)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_TemplateSection_Tenant");
            });

            modelBuilder.Entity<TemplateToDoStatus>(entity =>
            {
                entity.HasIndex(e => e.IsActive, "IX_TemplateToDoStatus_IsActive");

                entity.HasIndex(e => e.IsDefault, "IX_TemplateToDoStatus_IsDefault");

                entity.HasIndex(e => e.Name, "IX_TemplateToDoStatus_Name");

                entity.HasIndex(e => e.Type, "IX_TemplateToDoStatus_Type");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.TemplateNavigation)
                    .WithMany(p => p.TemplateToDoStatus)
                    .HasForeignKey(d => d.Template)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateToDoStatus_Template");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.TemplateToDoStatus)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_TemplateToDoStatus_Tenant");
            });

            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_Tenant_Name");

                entity.HasIndex(e => e.TenancyName, "UQ_Tenant_TenancyName")
                    .IsUnique();

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TenancyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TenantRegistration>(entity =>
            {
                entity.HasIndex(e => e.TenancyName, "UQ_TenantRegistration_TenancyName")
                    .IsUnique();

                entity.Property(e => e.ConfirmationCode)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SecondLastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenancyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenantName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ToDoActEvaluator>(entity =>
            {
                entity.HasOne(d => d.ToDoActivityNavigation)
                    .WithMany(p => p.ToDoActEvaluator)
                    .HasForeignKey(d => d.ToDoActivity)
                    .HasConstraintName("FK_ToDoActEvaluator_ToDoActivity");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.ToDoActEvaluator)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FK_ToDoActEvaluator_User");
            });

            modelBuilder.Entity<ToDoActExecutor>(entity =>
            {
                entity.HasOne(d => d.ToDoActivityNavigation)
                    .WithMany(p => p.ToDoActExecutor)
                    .HasForeignKey(d => d.ToDoActivity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ToDoActExecutor_ToDoActivity");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.ToDoActExecutor)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ToDoActExecutor_User");
            });

            modelBuilder.Entity<ToDoActivity>(entity =>
            {
                entity.HasIndex(e => e.IsOnTime, "IX_ToDoActivity_IsOnTime");

                entity.HasIndex(e => e.table, "IX_ToDoActivity_Table");

                entity.HasIndex(e => new { e.table, e.key }, "IX_ToDoActivity_TableKey");

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FinalPlannedDate).HasColumnType("date");

                entity.Property(e => e.FinalRealDate).HasColumnType("date");

                entity.Property(e => e.InitialPlannedDate).HasColumnType("date");

                entity.Property(e => e.InitialRealDate).HasColumnType("date");

                entity.Property(e => e.IsOnTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.table)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.ToDoActivity)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ToDoActivity_TemplateToDoStatus");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ToDoActivity)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_ToDoActivity_Tenant");

                entity.HasOne(d => d.UserCreatorNavigation)
                    .WithMany(p => p.ToDoActivity)
                    .HasForeignKey(d => d.UserCreator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ToDoActivity_User");
            });

            modelBuilder.Entity<ToDoTimeSheet>(entity =>
            {
                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreationDate).HasColumnType("date");

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.HoursSpend).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ToDoTimeSheet)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_ToDoTimeSheet_Tenant");

                entity.HasOne(d => d.ToDoActivityNavigation)
                    .WithMany(p => p.ToDoTimeSheet)
                    .HasForeignKey(d => d.ToDoActivity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ToDoTimeSheet_ToDoActivity");

                entity.HasOne(d => d.UserCreatorNavigation)
                    .WithMany(p => p.ToDoTimeSheet)
                    .HasForeignKey(d => d.UserCreator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ToDoTimeSheet_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => new { e.TenantId, e.UserLogin }, "UQ_User_UserLogin")
                    .IsUnique();

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LastLoginTime).HasColumnType("datetime");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SecondLastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_User_Tenant");
            });

            modelBuilder.Entity<UserARN>(entity =>
            {
                entity.HasIndex(e => e.UserId, "UQ_UserARN_UserId")
                    .IsUnique();

                entity.Property(e => e.ARNCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserARN)
                    .HasForeignKey<UserARN>(d => d.UserId)
                    .HasConstraintName("FK_UserARN_User");
            });

            modelBuilder.Entity<UserPasswordHistory>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPasswordHistory)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserPasswordHistory_User");
            });

            modelBuilder.Entity<UserReset>(entity =>
            {
                entity.HasIndex(e => e.UserId, "UQ_UserReset_UserId")
                    .IsUnique();

                entity.Property(e => e.ResetCode)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Validity).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserReset)
                    .HasForeignKey<UserReset>(d => d.UserId)
                    .HasConstraintName("FK_UserReset_User");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRole_User");
            });

            modelBuilder.Entity<help>(entity =>
            {
                entity.HasIndex(e => new { e.TenantId, e.HelpKey }, "IX_help_code");

                entity.HasIndex(e => new { e.LanguageId, e.HelpKey }, "UQ_help_HelpKey")
                    .IsUnique();

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HelpKey)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.help)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_help_Language");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.help)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_help_Tenant");
            });

            modelBuilder.Entity<helptxt>(entity =>
            {
                entity.HasIndex(e => e.help, "UQ_helptxt_help")
                    .IsUnique();

                entity.Property(e => e.body).IsUnicode(false);

                entity.HasOne(d => d.helpNavigation)
                    .WithOne(p => p.helptxt)
                    .HasForeignKey<helptxt>(d => d.help)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_helptxt_help");
            });

            modelBuilder.Entity<mailgroup>(entity =>
            {
                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.mailgroup)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_mailgroup_Tenant");
            });

            modelBuilder.Entity<mailgrouptxt>(entity =>
            {
                entity.Property(e => e.body).IsUnicode(false);

                entity.HasOne(d => d.mailgroupNavigation)
                    .WithMany(p => p.mailgrouptxt)
                    .HasForeignKey(d => d.mailgroup)
                    .HasConstraintName("FK_mailgrouptxt_mailgroup");
            });

            modelBuilder.Entity<mailtemplate>(entity =>
            {
                entity.HasIndex(e => new { e.mailgroup, e.mailkey }, "UQ_mailtemplate_mailkey")
                    .IsUnique();

                entity.Property(e => e.BlindCopyTo)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Body).IsUnicode(false);

                entity.Property(e => e.CopyTo)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SendTo)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.mailkey)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.mailtemplate)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_mailtemplate_Tenant");

                entity.HasOne(d => d.mailgroupNavigation)
                    .WithMany(p => p.mailtemplate)
                    .HasForeignKey(d => d.mailgroup)
                    .HasConstraintName("FK_mailtemplate_mailgroup");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        protected void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(typeof(SqlFunctions).GetMethod(nameof(SqlFunctions.AtTimeZone), new[] { typeof(DateTime?), typeof(string) }),
                b =>
                {
                    b.HasName("AtTimeZone");
                    b.HasParameter("dt").PropagatesNullability();
                });

            modelBuilder.HasDbFunction(typeof(SqlFunctions).GetMethod(nameof(SqlFunctions.CreateDateTime), new[] { typeof(DateTime?), typeof(TimeSpan?) }),
                b =>
                {
                    b.HasName("CreateDateTime");
                    b.HasParameter("dt").PropagatesNullability();
                });

            modelBuilder.HasDbFunction(typeof(SqlFunctions).GetMethod(nameof(SqlFunctions.DateDiffCustom), new[] { typeof(string), typeof(DateTime?), typeof(DateTime?) }),
            b =>
            {
                b.HasName("DateDiffCustom");
                b.HasParameter("unit").PropagatesNullability();
                b.HasParameter("dt1").PropagatesNullability();
                b.HasParameter("dt2").PropagatesNullability();
            });
        }
    }
}