PRINT N'Creando Tabla [dbo].[SettingClient]...';


GO
CREATE TABLE [dbo].[SettingClient] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantId]   INT            NULL,
    [UserId]     BIGINT         NULL,
    [ClientType] VARCHAR (50)   NULL,
    [Name]       VARCHAR (100)  NULL,
    [value]      VARCHAR (8000) NULL,
    CONSTRAINT [PK_SettingClient] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[SettingClient].[IX_SettingClient_ClientType]...';


GO
CREATE NONCLUSTERED INDEX [IX_SettingClient_ClientType]
    ON [dbo].[SettingClient]([ClientType] ASC, [Name] ASC);


GO
PRINT N'Creando Tabla [dbo].[SampleDateData]...';


GO
CREATE TABLE [dbo].[SampleDateData] (
    [Id]           BIGINT       IDENTITY (1, 1) NOT NULL,
    [TenantId]     INT          NULL,
    [Name]         VARCHAR (50) NULL,
    [DateTimeData] DATETIME     NULL,
    [DateData]     DATE         NULL,
    [TimeData]     TIME (7)     NULL,
    CONSTRAINT [PK_SampleDateData] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[mailtemplate]...';


GO
CREATE TABLE [dbo].[mailtemplate] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantId]    INT            NULL,
    [mailgroup]   BIGINT         NULL,
    [mailkey]     VARCHAR (50)   NULL,
    [DisplayName] VARCHAR (100)  NULL,
    [SendTo]      VARCHAR (1000) NULL,
    [CopyTo]      VARCHAR (1000) NULL,
    [BlindCopyTo] VARCHAR (1000) NULL,
    [Subject]     VARCHAR (250)  NULL,
    [Body]        VARCHAR (MAX)  NULL,
    [IsActive]    BIT            NULL,
    CONSTRAINT [PK_mailtemplate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_mailtemplate_mailkey] UNIQUE NONCLUSTERED ([mailgroup] ASC, [mailkey] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[mailgrouptxt]...';


GO
CREATE TABLE [dbo].[mailgrouptxt] (
    [Id]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [mailgroup] BIGINT        NULL,
    [type]      TINYINT       NULL,
    [body]      VARCHAR (MAX) NULL,
    CONSTRAINT [PK_mailgrouptxt] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[mailgroup]...';


GO
CREATE TABLE [dbo].[mailgroup] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantId]    INT           NULL,
    [DisplayName] VARCHAR (100) NULL,
    CONSTRAINT [PK_mailgroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[__EFMigrationsHistory]...';


GO
CREATE TABLE [dbo].[__EFMigrationsHistory] (
    [MigrationId]    NVARCHAR (150) NOT NULL,
    [ProductVersion] NVARCHAR (32)  NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC) ON [PRIMARY]
) ON [PRIMARY];


GO
PRINT N'Creando Tabla [dbo].[ChangeLogDetail]...';


GO
CREATE TABLE [dbo].[ChangeLogDetail] (
    [Id]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [changelog] BIGINT        NULL,
    [data]      VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ChangeLogDetail] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[ChangeLog]...';


GO
CREATE TABLE [dbo].[ChangeLog] (
    [Id]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [UserId]   BIGINT       NULL,
    [TenantId] INT          NULL,
    [table]    VARCHAR (30) NULL,
    [key]      VARCHAR (12) NULL,
    [datetime] DATETIME     NULL,
    CONSTRAINT [PK_ChangeLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[ChangeLog].[IX_ChangeLog_Table]...';


GO
CREATE NONCLUSTERED INDEX [IX_ChangeLog_Table]
    ON [dbo].[ChangeLog]([table] ASC);


GO
PRINT N'Creando Índice [dbo].[ChangeLog].[IX_ChangeLog_TableKey]...';


GO
CREATE NONCLUSTERED INDEX [IX_ChangeLog_TableKey]
    ON [dbo].[ChangeLog]([table] ASC, [key] ASC);


GO
PRINT N'Creando Tabla [dbo].[AuditLog]...';


GO
CREATE TABLE [dbo].[AuditLog] (
    [Id]                   BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantId]             INT            NULL,
    [UserId]               BIGINT         NULL,
    [ImpersonalizerUserId] BIGINT         NULL,
    [ServiceName]          VARCHAR (250)  NULL,
    [MethodName]           VARCHAR (250)  NULL,
    [Parameters]           NVARCHAR (MAX) NULL,
    [ExecutionDatetime]    DATETIME       NULL,
    [ExecutionDuration]    INT            NULL,
    [ClientIpAddress]      VARCHAR (64)   NULL,
    [ClientName]           VARCHAR (128)  NULL,
    [BroserInfo]           VARCHAR (250)  NULL,
    [Exception]            VARCHAR (MAX)  NULL,
    [CustomData]           NVARCHAR (MAX) NULL,
    [Severity]             TINYINT        NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[AuditLog].[IX_AuditLog_ExecutionDatetime]...';


GO
CREATE NONCLUSTERED INDEX [IX_AuditLog_ExecutionDatetime]
    ON [dbo].[AuditLog]([TenantId] ASC, [ExecutionDatetime] ASC);


GO
PRINT N'Creando Índice [dbo].[AuditLog].[IX_AuditLog_Severity]...';


GO
CREATE NONCLUSTERED INDEX [IX_AuditLog_Severity]
    ON [dbo].[AuditLog]([TenantId] ASC, [Severity] ASC);


GO
PRINT N'Creando Índice [dbo].[AuditLog].[IX_AuditLog_ServiceName]...';


GO
CREATE NONCLUSTERED INDEX [IX_AuditLog_ServiceName]
    ON [dbo].[AuditLog]([TenantId] ASC, [ServiceName] ASC);


GO
PRINT N'Creando Índice [dbo].[AuditLog].[IX_AuditLog_MethodName]...';


GO
CREATE NONCLUSTERED INDEX [IX_AuditLog_MethodName]
    ON [dbo].[AuditLog]([TenantId] ASC, [MethodName] ASC);


GO
PRINT N'Creando Índice [dbo].[AuditLog].[IX_AuditLog_ExecutionDuration]...';


GO
CREATE NONCLUSTERED INDEX [IX_AuditLog_ExecutionDuration]
    ON [dbo].[AuditLog]([TenantId] ASC, [ExecutionDuration] ASC);


GO
PRINT N'Creando Tabla [dbo].[Setting]...';


GO
CREATE TABLE [dbo].[Setting] (
    [Id]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantId] INT            NULL,
    [UserId]   BIGINT         NULL,
    [Name]     VARCHAR (100)  NULL,
    [value]    VARCHAR (8000) NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[Setting].[IX_Setting_Name]...';


GO
CREATE NONCLUSTERED INDEX [IX_Setting_Name]
    ON [dbo].[Setting]([Name] ASC);


GO
PRINT N'Creando Tabla [dbo].[LanguageText]...';


GO
CREATE TABLE [dbo].[LanguageText] (
    [Id]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [LanguageId] INT           NULL,
    [Key]        VARCHAR (100) NULL,
    [Value]      VARCHAR (MAX) NULL,
    CONSTRAINT [PK_LanguageText] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_LanguageText_Key] UNIQUE NONCLUSTERED ([LanguageId] ASC, [Key] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[Language]...';


GO
CREATE TABLE [dbo].[Language] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [TenantId]    INT           NULL,
    [Name]        VARCHAR (10)  NULL,
    [DisplayName] VARCHAR (100) NULL,
    [IsActive]    BIT           NULL,
    CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[UserRole]...';


GO
CREATE TABLE [dbo].[UserRole] (
    [Id]     BIGINT IDENTITY (1, 1) NOT NULL,
    [UserId] BIGINT NULL,
    [RoleId] BIGINT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[Role]...';


GO
CREATE TABLE [dbo].[Role] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantId]    INT           NULL,
    [Name]        VARCHAR (50)  NULL,
    [DisplayName] VARCHAR (100) NULL,
    [IsActive]    BIT           NULL,
    [IsDeleted]   BIT           NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[Permission]...';


GO
CREATE TABLE [dbo].[Permission] (
    [Id]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [Role]      BIGINT        NULL,
    [Name]      VARCHAR (150) NULL,
    [IsGranted] BIT           NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Permission_RolePermission] UNIQUE NONCLUSTERED ([Role] ASC, [Name] ASC)
);


GO
PRINT N'Creando Índice [dbo].[Permission].[IX_Permission_Name]...';


GO
CREATE NONCLUSTERED INDEX [IX_Permission_Name]
    ON [dbo].[Permission]([Name] ASC);


GO
PRINT N'Creando Tabla [dbo].[UserReset]...';


GO
CREATE TABLE [dbo].[UserReset] (
    [Id]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [UserId]    BIGINT       NULL,
    [ResetCode] VARCHAR (40) NULL,
    [Validity]  DATETIME     NULL,
    CONSTRAINT [PK_UserReset] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_UserReset_UserId] UNIQUE NONCLUSTERED ([UserId] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[UserPasswordHistory]...';


GO
CREATE TABLE [dbo].[UserPasswordHistory] (
    [Id]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]   BIGINT        NULL,
    [Password] VARCHAR (200) NULL,
    [Date]     DATETIME      NULL,
    CONSTRAINT [PK_UserPasswordHistory] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[UserARN]...';


GO
CREATE TABLE [dbo].[UserARN] (
    [Id]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]  BIGINT        NULL,
    [ARNCode] VARCHAR (200) NULL,
    CONSTRAINT [PK_UserARN] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_UserARN_UserId] UNIQUE NONCLUSTERED ([UserId] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[User]...';


GO
CREATE TABLE [dbo].[User] (
    [Id]                BIGINT           IDENTITY (1, 1) NOT NULL,
    [TenantId]          INT              NULL,
    [UserLogin]         VARCHAR (32)     NULL,
    [Password]          VARCHAR (200)    NULL,
    [Name]              VARCHAR (50)     NULL,
    [Lastname]          VARCHAR (50)     NULL,
    [SecondLastname]    VARCHAR (50)     NULL,
    [EmailAddress]      VARCHAR (250)    NULL,
    [IsEmailConfirmed]  BIT              NULL,
    [PhoneNumber]       VARCHAR (20)     NULL,
    [IsPhoneConfirmed]  BIT              NULL,
    [CreationTime]      DATETIME         NULL,
    [ChangePassword]    BIT              NULL,
    [AccessFailedCount] INT              NULL,
    [LastLoginTime]     DATETIME         NULL,
    [ProfilePictureId]  UNIQUEIDENTIFIER NULL,
    [UserLocked]        BIT              NULL,
    [IsLockoutEnabled]  BIT              NULL,
    [IsActive]          BIT              NULL,
    [IsDeleted]         BIT              NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_User_UserLogin] UNIQUE NONCLUSTERED ([TenantId] ASC, [UserLogin] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[TenantRegistration]...';


GO
CREATE TABLE [dbo].[TenantRegistration] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [TenancyName]      VARCHAR (50)  NULL,
    [TenantName]       VARCHAR (150) NULL,
    [UserLogin]        VARCHAR (32)  NULL,
    [Password]         VARCHAR (200) NULL,
    [Name]             VARCHAR (50)  NULL,
    [Lastname]         VARCHAR (50)  NULL,
    [SecondLastname]   VARCHAR (50)  NULL,
    [EmailAddress]     VARCHAR (250) NULL,
    [ConfirmationCode] VARCHAR (40)  NULL,
    CONSTRAINT [PK_TenantRegistration] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_TenantRegistration_TenancyName] UNIQUE NONCLUSTERED ([TenancyName] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[Tenant]...';


GO
CREATE TABLE [dbo].[Tenant] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [TenancyName]  VARCHAR (50)  NULL,
    [Name]         VARCHAR (150) NULL,
    [CreationTime] DATETIME      NULL,
    [IsActive]     BIT           NULL,
    [IsDeleted]    BIT           NULL,
    CONSTRAINT [PK_tenant] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Tenant_TenancyName] UNIQUE NONCLUSTERED ([TenancyName] ASC)
);


GO
PRINT N'Creando Índice [dbo].[Tenant].[IX_Tenant_Name]...';


GO
CREATE NONCLUSTERED INDEX [IX_Tenant_Name]
    ON [dbo].[Tenant]([Name] ASC);


GO
PRINT N'Creando Tabla [dbo].[BinaryObjects]...';


GO
CREATE TABLE [dbo].[BinaryObjects] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [TenantId] INT              NULL,
    [Bytes]    VARBINARY (MAX)  NULL,
    [FileName] VARCHAR (250)    NULL,
    CONSTRAINT [PK_BinaryObjects] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[ToDoActExecutor]...';


GO
CREATE TABLE [dbo].[ToDoActExecutor] (
    [Id]           BIGINT IDENTITY (1, 1) NOT NULL,
    [ToDoActivity] BIGINT NOT NULL,
    [User]         BIGINT NOT NULL,
    CONSTRAINT [PK_ToDoActExecutor] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[ToDoActivity]...';


GO
CREATE TABLE [dbo].[ToDoActivity] (
    [Id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantId]           INT           NULL,
    [UserCreator]        BIGINT        NOT NULL,
    [Status]             BIGINT        NOT NULL,
    [CreationTime]       DATETIME      NULL,
    [Description]        VARCHAR (100) NULL,
    [InitialPlannedDate] DATE          NULL,
    [InitialRealDate]    DATE          NULL,
    [FinalPlannedDate]   DATE          NULL,
    [FinalRealDate]      DATE          NULL,
    [IsOnTime]           BIT           NULL,
    [table]              VARCHAR (50)  NULL,
    [key]                BIGINT        NULL,
    CONSTRAINT [PK_ToDoActivity] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[ToDoActivity].[IX_ToDoActivity_Table]...';


GO
CREATE NONCLUSTERED INDEX [IX_ToDoActivity_Table]
    ON [dbo].[ToDoActivity]([table] ASC);


GO
PRINT N'Creando Índice [dbo].[ToDoActivity].[IX_ToDoActivity_TableKey]...';


GO
CREATE NONCLUSTERED INDEX [IX_ToDoActivity_TableKey]
    ON [dbo].[ToDoActivity]([table] ASC, [key] ASC);


GO
PRINT N'Creando Índice [dbo].[ToDoActivity].[IX_ToDoActivity_IsOnTime]...';


GO
CREATE NONCLUSTERED INDEX [IX_ToDoActivity_IsOnTime]
    ON [dbo].[ToDoActivity]([IsOnTime] ASC);


GO
PRINT N'Creando Tabla [dbo].[TemplateToDoStatus]...';


GO
CREATE TABLE [dbo].[TemplateToDoStatus] (
    [Id]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [Template]  BIGINT       NOT NULL,
    [TenantId]  INT          NULL,
    [Type]      TINYINT      NULL,
    [Name]      VARCHAR (30) NULL,
    [IsDefault] BIT          NULL,
    [IsActive]  BIT          NULL,
    [IsDeleted] BIT          NULL,
    CONSTRAINT [PK_TemplateToDoStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[TemplateToDoStatus].[IX_TemplateToDoStatus_Name]...';


GO
CREATE NONCLUSTERED INDEX [IX_TemplateToDoStatus_Name]
    ON [dbo].[TemplateToDoStatus]([Name] ASC);


GO
PRINT N'Creando Índice [dbo].[TemplateToDoStatus].[IX_TemplateToDoStatus_Type]...';


GO
CREATE NONCLUSTERED INDEX [IX_TemplateToDoStatus_Type]
    ON [dbo].[TemplateToDoStatus]([Type] ASC);


GO
PRINT N'Creando Índice [dbo].[TemplateToDoStatus].[IX_TemplateToDoStatus_IsDefault]...';


GO
CREATE NONCLUSTERED INDEX [IX_TemplateToDoStatus_IsDefault]
    ON [dbo].[TemplateToDoStatus]([IsDefault] ASC);


GO
PRINT N'Creando Índice [dbo].[TemplateToDoStatus].[IX_TemplateToDoStatus_IsActive]...';


GO
CREATE NONCLUSTERED INDEX [IX_TemplateToDoStatus_IsActive]
    ON [dbo].[TemplateToDoStatus]([IsActive] ASC);


GO
PRINT N'Creando Tabla [dbo].[OrgUnitUser]...';


GO
CREATE TABLE [dbo].[OrgUnitUser] (
    [Id]      BIGINT IDENTITY (1, 1) NOT NULL,
    [OrgUnit] BIGINT NOT NULL,
    [User]    BIGINT NOT NULL,
    CONSTRAINT [PK_OrgUnitUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[ChatRoomChatFile]...';


GO
CREATE TABLE [dbo].[ChatRoomChatFile] (
    [Id]            BIGINT           IDENTITY (1, 1) NOT NULL,
    [ChatRoomChat]  BIGINT           NOT NULL,
    [FileName]      VARCHAR (250)    NULL,
    [FileExtension] VARCHAR (15)     NULL,
    [File]          UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_ChatRoomChatFile] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[ChatRoomChatUserTagged]...';


GO
CREATE TABLE [dbo].[ChatRoomChatUserTagged] (
    [Id]           BIGINT IDENTITY (1, 1) NOT NULL,
    [ChatRoomChat] BIGINT NOT NULL,
    [UserTagged]   BIGINT NOT NULL,
    CONSTRAINT [PK_ChatRoomChatUserTagged] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[ChatRoomChat]...';


GO
CREATE TABLE [dbo].[ChatRoomChat] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantId]     INT           NULL,
    [ChatRoom]     BIGINT        NOT NULL,
    [User]         BIGINT        NULL,
    [CreationTime] DATETIME      NULL,
    [Comment]      VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ChatRoomChat] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[ChatRoomChat].[IX_ChatRoomChat_CreationTime]...';


GO
CREATE NONCLUSTERED INDEX [IX_ChatRoomChat_CreationTime]
    ON [dbo].[ChatRoomChat]([TenantId] ASC, [CreationTime] ASC);


GO
PRINT N'Creando Tabla [dbo].[ChatRoom]...';


GO
CREATE TABLE [dbo].[ChatRoom] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantId]     INT           NULL,
    [UserCreator]  BIGINT        NULL,
    [ChatRoomId]   VARCHAR (50)  NOT NULL,
    [Name]         VARCHAR (50)  NOT NULL,
    [CreationTime] DATETIME      NULL,
    [Description]  VARCHAR (250) NULL,
    CONSTRAINT [PK_ChatRoom] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_ChatRoom_ChatRoomId] UNIQUE NONCLUSTERED ([TenantId] ASC, [ChatRoomId] ASC)
);


GO
PRINT N'Creando Índice [dbo].[ChatRoom].[IX_ChatRoom_ChatRoomId]...';


GO
CREATE NONCLUSTERED INDEX [IX_ChatRoom_ChatRoomId]
    ON [dbo].[ChatRoom]([TenantId] ASC, [ChatRoomId] ASC);


GO
PRINT N'Creando Índice [dbo].[ChatRoom].[IX_ChatRoom_Name]...';


GO
CREATE NONCLUSTERED INDEX [IX_ChatRoom_Name]
    ON [dbo].[ChatRoom]([TenantId] ASC, [Name] ASC);


GO
PRINT N'Creando Índice [dbo].[ChatRoom].[IX_ChatRoom_Description]...';


GO
CREATE NONCLUSTERED INDEX [IX_ChatRoom_Description]
    ON [dbo].[ChatRoom]([TenantId] ASC, [Description] ASC);


GO
PRINT N'Creando Índice [dbo].[ChatRoom].[IX_ChatRoom_CreationTime]...';


GO
CREATE NONCLUSTERED INDEX [IX_ChatRoom_CreationTime]
    ON [dbo].[ChatRoom]([TenantId] ASC, [CreationTime] ASC);


GO
PRINT N'Creando Tabla [dbo].[TemplateQuery]...';


GO
CREATE TABLE [dbo].[TemplateQuery] (
    [Id]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [Template]  BIGINT        NULL,
    [QueryType] TINYINT       NULL,
    [Query]     VARCHAR (MAX) NULL,
    CONSTRAINT [PK_TemplateQuery] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_TemplateQuery_QueryType] UNIQUE NONCLUSTERED ([Template] ASC, [QueryType] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[TemplateSection]...';


GO
CREATE TABLE [dbo].[TemplateSection] (
    [Id]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [TenantId]  INT          NULL,
    [Template]  BIGINT       NULL,
    [Name]      VARCHAR (50) NULL,
    [Order]     SMALLINT     NULL,
    [IconAF]    VARCHAR (20) NULL,
    [IsDeleted] BIT          NULL,
    CONSTRAINT [PK_TemplateSection] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[TemplateFieldRelation]...';


GO
CREATE TABLE [dbo].[TemplateFieldRelation] (
    [Id]                    BIGINT IDENTITY (1, 1) NOT NULL,
    [TenantId]              INT    NULL,
    [TemplateField]         BIGINT NOT NULL,
    [TemplateFieldRelation] BIGINT NOT NULL,
    CONSTRAINT [PK_TemplateFieldRelation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_TemplateFieldRelation_TemplateField] UNIQUE NONCLUSTERED ([TemplateField] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[TemplateFieldOption]...';


GO
CREATE TABLE [dbo].[TemplateFieldOption] (
    [Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [TemplateField] BIGINT        NULL,
    [value]         INT           NOT NULL,
    [Description]   VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_TemplateFieldOption] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_TemplateFieldOption_value] UNIQUE NONCLUSTERED ([TemplateField] ASC, [value] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[TemplateField]...';


GO
CREATE TABLE [dbo].[TemplateField] (
    [Id]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantId]        INT           NULL,
    [TemplateSection] BIGINT        NULL,
    [Name]            VARCHAR (30)  NULL,
    [FieldName]       VARCHAR (30)  NULL,
    [FieldType]       SMALLINT      NULL,
    [FieldSize]       SMALLINT      NULL,
    [FieldControl]    SMALLINT      NULL,
    [InputMask]       VARCHAR (50)  NULL,
    [HasKeyFilter]    BIT           NULL,
    [KeyFilter]       VARCHAR (500) NULL,
    [Status]          TINYINT       NULL,
    [IsRequired]      BIT           NULL,
    [ShowOnGrid]      BIT           NULL,
    [Order]           SMALLINT      NULL,
    [InheritSecurity] BIT           NULL,
    [IsDeleted]       BIT           NULL,
    CONSTRAINT [PK_TemplateField] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[Template]...';


GO
CREATE TABLE [dbo].[Template] (
    [Id]               BIGINT           IDENTITY (1, 1) NOT NULL,
    [TenantId]         INT              NULL,
    [RGBColor]         VARCHAR (6)      NULL,
    [NameSingular]     VARCHAR (20)     NOT NULL,
    [NamePlural]       VARCHAR (22)     NOT NULL,
    [Description]      VARCHAR (1000)   NULL,
    [Icon]             UNIQUEIDENTIFIER NULL,
    [TableName]        VARCHAR (50)     NULL,
    [IsTableGenerated] BIT              NULL,
    [HasChatRoom]      BIT              NULL,
    [IsActivity]       BIT              NULL,
    [HasSecurity]      BIT              NULL,
    [IsActive]         BIT              NULL,
    [IsDeleted]        BIT              NULL,
    CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Template_TableName] UNIQUE NONCLUSTERED ([TableName] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[helptxt]...';


GO
CREATE TABLE [dbo].[helptxt] (
    [Id]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [help] BIGINT        NOT NULL,
    [body] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_helptxt] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_helptxt_help] UNIQUE NONCLUSTERED ([help] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[help]...';


GO
CREATE TABLE [dbo].[help] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantId]    INT           NULL,
    [LanguageId]  INT           NOT NULL,
    [HelpKey]     VARCHAR (50)  NOT NULL,
    [DisplayName] VARCHAR (100) NULL,
    [IsActive]    BIT           NULL,
    CONSTRAINT [PK_help] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_help_HelpKey] UNIQUE NONCLUSTERED ([LanguageId] ASC, [HelpKey] ASC)
);


GO
PRINT N'Creando Índice [dbo].[help].[IX_help_code]...';


GO
CREATE NONCLUSTERED INDEX [IX_help_code]
    ON [dbo].[help]([TenantId] ASC, [HelpKey] ASC);


GO
PRINT N'Creando Tabla [dbo].[ChatMessage]...';


GO
CREATE TABLE [dbo].[ChatMessage] (
    [Id]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantId]       INT           NULL,
    [UserId]         BIGINT        NOT NULL,
    [FriendTenantId] INT           NULL,
    [FriendUserId]   BIGINT        NOT NULL,
    [CreationTime]   DATETIME      NULL,
    [Message]        VARCHAR (MAX) NULL,
    [State]          TINYINT       NULL,
    [Side]           TINYINT       NULL,
    CONSTRAINT [PK_ChatMessage] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[ChatMessage].[IX_ChatMessage_UserId]...';


GO
CREATE NONCLUSTERED INDEX [IX_ChatMessage_UserId]
    ON [dbo].[ChatMessage]([TenantId] ASC, [UserId] ASC);


GO
PRINT N'Creando Índice [dbo].[ChatMessage].[IX_ChatMessage_CreationTime]...';


GO
CREATE NONCLUSTERED INDEX [IX_ChatMessage_CreationTime]
    ON [dbo].[ChatMessage]([CreationTime] ASC);


GO
PRINT N'Creando Tabla [dbo].[Friendship]...';


GO
CREATE TABLE [dbo].[Friendship] (
    [Id]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [TenantId]       INT          NULL,
    [UserId]         BIGINT       NOT NULL,
    [FriendTenantId] INT          NULL,
    [FriendUserId]   BIGINT       NOT NULL,
    [CreationTime]   DATETIME     NULL,
    [FriendNickname] VARCHAR (50) NULL,
    [State]          TINYINT      NULL,
    CONSTRAINT [PK_Friendship] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[Friendship].[IX_Friendship_UserId]...';


GO
CREATE NONCLUSTERED INDEX [IX_Friendship_UserId]
    ON [dbo].[Friendship]([TenantId] ASC, [UserId] ASC);


GO
PRINT N'Creando Índice [dbo].[Friendship].[IX_Friendship_CreationTime]...';


GO
CREATE NONCLUSTERED INDEX [IX_Friendship_CreationTime]
    ON [dbo].[Friendship]([CreationTime] ASC);


GO
PRINT N'Creando Tabla [dbo].[MailServiceMailAttach]...';


GO
CREATE TABLE [dbo].[MailServiceMailAttach] (
    [Id]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [MailServiceMailBody] BIGINT          NULL,
    [ContenType]          VARCHAR (50)    NULL,
    [FileName]            VARCHAR (200)   NULL,
    [BinaryFile]          VARBINARY (MAX) NULL,
    CONSTRAINT [PK_MailServiceMailAttach] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[MailServiceMailBody]...';


GO
CREATE TABLE [dbo].[MailServiceMailBody] (
    [Id]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [MailServiceMail] BIGINT        NOT NULL,
    [Body]            VARCHAR (MAX) NULL,
    CONSTRAINT [PK_MailServiceMailBody] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_MailServiceMailBody_MailServiceMail] UNIQUE NONCLUSTERED ([MailServiceMail] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[MailServiceMailStatus]...';


GO
CREATE TABLE [dbo].[MailServiceMailStatus] (
    [Id]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [MailServiceMail] BIGINT        NOT NULL,
    [SentTime]        DATETIME      NULL,
    [Status]          TINYINT       NOT NULL,
    [Error]           VARCHAR (MAX) NULL,
    CONSTRAINT [PK_MailServiceMailStatus] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_MailServiceMailStatus_MailServiceMail] UNIQUE NONCLUSTERED ([MailServiceMail] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[MailServiceMailConfig]...';


GO
CREATE TABLE [dbo].[MailServiceMailConfig] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [MailServiceMail]      BIGINT        NOT NULL,
    [Sender]               VARCHAR (250) NULL,
    [SenderDisplay]        VARCHAR (250) NULL,
    [SMPTHost]             VARCHAR (100) NULL,
    [SMPTPort]             SMALLINT      NULL,
    [IsSSL]                BIT           NULL,
    [UseDefaultCredential] BIT           NULL,
    [Domain]               VARCHAR (100) NULL,
    [MailUser]             VARCHAR (50)  NULL,
    [MailPassword]         VARCHAR (100) NULL,
    CONSTRAINT [PK_MailServiceMailConfig] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_MailServiceMailConfig_MailServiceMail] UNIQUE NONCLUSTERED ([MailServiceMail] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[MailServiceMail]...';


GO
CREATE TABLE [dbo].[MailServiceMail] (
    [Id]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantId]           INT            NOT NULL,
    [MailServiceRequest] BIGINT         NOT NULL,
    [IsLocalConfig]      BIT            NOT NULL,
    [Sendto]             VARCHAR (1000) NULL,
    [CopyTo]             VARCHAR (1000) NULL,
    [BlindCopyTo]        VARCHAR (1000) NULL,
    [Subject]            VARCHAR (500)  NULL,
    CONSTRAINT [PK_MailServiceMail] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[MailServiceRequest]...';


GO
CREATE TABLE [dbo].[MailServiceRequest] (
    [Id]           BIGINT   IDENTITY (1, 1) NOT NULL,
    [TenantId]     INT      NOT NULL,
    [UserCreator]  BIGINT   NOT NULL,
    [CreationTime] DATETIME NULL,
    CONSTRAINT [PK_MailServiceRequest] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_MailServiceRequest_TenantId] UNIQUE NONCLUSTERED ([TenantId] ASC, [Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[ToDoTimeSheet]...';


GO
CREATE TABLE [dbo].[ToDoTimeSheet] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantId]     INT             NULL,
    [ToDoActivity] BIGINT          NOT NULL,
    [UserCreator]  BIGINT          NOT NULL,
    [CreationDate] DATE            NULL,
    [CreationTime] DATETIME        NULL,
    [Comments]     VARCHAR (250)   NULL,
    [HoursSpend]   DECIMAL (10, 2) NULL,
    CONSTRAINT [PK_ToDoTimeSheet] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[TemplateDefaultOUEditor]...';


GO
CREATE TABLE [dbo].[TemplateDefaultOUEditor] (
    [Id]         BIGINT IDENTITY (1, 1) NOT NULL,
    [Template]   BIGINT NOT NULL,
    [OrgUnit]    BIGINT NOT NULL,
    [IsExecutor] BIT    NOT NULL,
    CONSTRAINT [PK_TemplateDefaultOUEditor] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[TemplateDefaultOUEditor].[IX_TemplateDefaultOUEditor_IsExecutor]...';


GO
CREATE NONCLUSTERED INDEX [IX_TemplateDefaultOUEditor_IsExecutor]
    ON [dbo].[TemplateDefaultOUEditor]([IsExecutor] ASC);


GO
PRINT N'Creando Tabla [dbo].[TemplateDefaultOUReader]...';


GO
CREATE TABLE [dbo].[TemplateDefaultOUReader] (
    [Id]       BIGINT IDENTITY (1, 1) NOT NULL,
    [Template] BIGINT NOT NULL,
    [OrgUnit]  BIGINT NOT NULL,
    CONSTRAINT [PK_TemplateDefaultOUReader] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[TemplateDefaultUserEditor]...';


GO
CREATE TABLE [dbo].[TemplateDefaultUserEditor] (
    [Id]         BIGINT IDENTITY (1, 1) NOT NULL,
    [Template]   BIGINT NOT NULL,
    [User]       BIGINT NOT NULL,
    [IsExecutor] BIT    NOT NULL,
    CONSTRAINT [PK_TemplateDefaultUserEditor] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[TemplateDefaultUserEditor].[IX_TemplateDefaultUserEditor_IsExecutor]...';


GO
CREATE NONCLUSTERED INDEX [IX_TemplateDefaultUserEditor_IsExecutor]
    ON [dbo].[TemplateDefaultUserEditor]([IsExecutor] ASC);


GO
PRINT N'Creando Tabla [dbo].[TemplateDefaultUserReader]...';


GO
CREATE TABLE [dbo].[TemplateDefaultUserReader] (
    [Id]       BIGINT IDENTITY (1, 1) NOT NULL,
    [Template] BIGINT NOT NULL,
    [User]     BIGINT NOT NULL,
    CONSTRAINT [PK_TemplateDefaultUserReader] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[ToDoActEvaluator]...';


GO
CREATE TABLE [dbo].[ToDoActEvaluator] (
    [Id]           BIGINT IDENTITY (1, 1) NOT NULL,
    [ToDoActivity] BIGINT NULL,
    [User]         BIGINT NULL,
    CONSTRAINT [PK_ToDoActEvaluator] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[OrgUnit]...';


GO
CREATE TABLE [dbo].[OrgUnit] (
    [Id]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantId]  INT           NULL,
    [ParentOU]  BIGINT        NULL,
    [Name]      VARCHAR (100) NOT NULL,
    [Level]     TINYINT       NOT NULL,
    [IsDeleted] BIT           NULL,
    CONSTRAINT [PK_OrgUnit] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Índice [dbo].[OrgUnit].[IX_OrgUnit_Name]...';


GO
CREATE NONCLUSTERED INDEX [IX_OrgUnit_Name]
    ON [dbo].[OrgUnit]([TenantId] ASC, [Name] ASC);


GO
PRINT N'Creando Índice [dbo].[OrgUnit].[IX_OrgUnit_IsDeleted]...';


GO
CREATE NONCLUSTERED INDEX [IX_OrgUnit_IsDeleted]
    ON [dbo].[OrgUnit]([IsDeleted] ASC);


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[ToDoActivity]...';


GO
ALTER TABLE [dbo].[ToDoActivity]
    ADD DEFAULT 0 FOR [IsOnTime];


GO
PRINT N'Creando Clave externa [dbo].[FK_SettingClient_Tenant]...';


GO
ALTER TABLE [dbo].[SettingClient]
    ADD CONSTRAINT [FK_SettingClient_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_SettingClient_User]...';


GO
ALTER TABLE [dbo].[SettingClient]
    ADD CONSTRAINT [FK_SettingClient_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_SampleDateData_Tenant]...';


GO
ALTER TABLE [dbo].[SampleDateData]
    ADD CONSTRAINT [FK_SampleDateData_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_mailtemplate_mailgroup]...';


GO
ALTER TABLE [dbo].[mailtemplate]
    ADD CONSTRAINT [FK_mailtemplate_mailgroup] FOREIGN KEY ([mailgroup]) REFERENCES [dbo].[mailgroup] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_mailtemplate_Tenant]...';


GO
ALTER TABLE [dbo].[mailtemplate]
    ADD CONSTRAINT [FK_mailtemplate_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_mailgrouptxt_mailgroup]...';


GO
ALTER TABLE [dbo].[mailgrouptxt]
    ADD CONSTRAINT [FK_mailgrouptxt_mailgroup] FOREIGN KEY ([mailgroup]) REFERENCES [dbo].[mailgroup] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_mailgroup_Tenant]...';


GO
ALTER TABLE [dbo].[mailgroup]
    ADD CONSTRAINT [FK_mailgroup_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChangeLogDetail_ChangeLog]...';


GO
ALTER TABLE [dbo].[ChangeLogDetail]
    ADD CONSTRAINT [FK_ChangeLogDetail_ChangeLog] FOREIGN KEY ([changelog]) REFERENCES [dbo].[ChangeLog] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChangeLog_Tenant]...';


GO
ALTER TABLE [dbo].[ChangeLog]
    ADD CONSTRAINT [FK_ChangeLog_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChangeLog_User]...';


GO
ALTER TABLE [dbo].[ChangeLog]
    ADD CONSTRAINT [FK_ChangeLog_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_AuditLog_User]...';


GO
ALTER TABLE [dbo].[AuditLog]
    ADD CONSTRAINT [FK_AuditLog_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_AuditLog_Tenant]...';


GO
ALTER TABLE [dbo].[AuditLog]
    ADD CONSTRAINT [FK_AuditLog_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_AuditLog_ImpersonalizerUserId]...';


GO
ALTER TABLE [dbo].[AuditLog]
    ADD CONSTRAINT [FK_AuditLog_ImpersonalizerUserId] FOREIGN KEY ([ImpersonalizerUserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_Setting_Tenant]...';


GO
ALTER TABLE [dbo].[Setting]
    ADD CONSTRAINT [FK_Setting_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_Setting_User]...';


GO
ALTER TABLE [dbo].[Setting]
    ADD CONSTRAINT [FK_Setting_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_LanguageText_Language]...';


GO
ALTER TABLE [dbo].[LanguageText]
    ADD CONSTRAINT [FK_LanguageText_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_Language_Tenant]...';


GO
ALTER TABLE [dbo].[Language]
    ADD CONSTRAINT [FK_Language_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_UserRole_Role]...';


GO
ALTER TABLE [dbo].[UserRole]
    ADD CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_UserRole_User]...';


GO
ALTER TABLE [dbo].[UserRole]
    ADD CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_Role_Tenant]...';


GO
ALTER TABLE [dbo].[Role]
    ADD CONSTRAINT [FK_Role_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_Permission_Role]...';


GO
ALTER TABLE [dbo].[Permission]
    ADD CONSTRAINT [FK_Permission_Role] FOREIGN KEY ([Role]) REFERENCES [dbo].[Role] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_UserReset_User]...';


GO
ALTER TABLE [dbo].[UserReset]
    ADD CONSTRAINT [FK_UserReset_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_UserPasswordHistory_User]...';


GO
ALTER TABLE [dbo].[UserPasswordHistory]
    ADD CONSTRAINT [FK_UserPasswordHistory_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_UserARN_User]...';


GO
ALTER TABLE [dbo].[UserARN]
    ADD CONSTRAINT [FK_UserARN_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_User_Tenant]...';


GO
ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [FK_User_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_BinaryObjects_Tenant]...';


GO
ALTER TABLE [dbo].[BinaryObjects]
    ADD CONSTRAINT [FK_BinaryObjects_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ToDoActExecutor_ToDoActivity]...';


GO
ALTER TABLE [dbo].[ToDoActExecutor]
    ADD CONSTRAINT [FK_ToDoActExecutor_ToDoActivity] FOREIGN KEY ([ToDoActivity]) REFERENCES [dbo].[ToDoActivity] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ToDoActExecutor_User]...';


GO
ALTER TABLE [dbo].[ToDoActExecutor]
    ADD CONSTRAINT [FK_ToDoActExecutor_User] FOREIGN KEY ([User]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ToDoActivity_TemplateToDoStatus]...';


GO
ALTER TABLE [dbo].[ToDoActivity]
    ADD CONSTRAINT [FK_ToDoActivity_TemplateToDoStatus] FOREIGN KEY ([Status]) REFERENCES [dbo].[TemplateToDoStatus] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ToDoActivity_Tenant]...';


GO
ALTER TABLE [dbo].[ToDoActivity]
    ADD CONSTRAINT [FK_ToDoActivity_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ToDoActivity_User]...';


GO
ALTER TABLE [dbo].[ToDoActivity]
    ADD CONSTRAINT [FK_ToDoActivity_User] FOREIGN KEY ([UserCreator]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateToDoStatus_Template]...';


GO
ALTER TABLE [dbo].[TemplateToDoStatus]
    ADD CONSTRAINT [FK_TemplateToDoStatus_Template] FOREIGN KEY ([Template]) REFERENCES [dbo].[Template] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateToDoStatus_Tenant]...';


GO
ALTER TABLE [dbo].[TemplateToDoStatus]
    ADD CONSTRAINT [FK_TemplateToDoStatus_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_OrgUnitUser_OrgUnit]...';


GO
ALTER TABLE [dbo].[OrgUnitUser]
    ADD CONSTRAINT [FK_OrgUnitUser_OrgUnit] FOREIGN KEY ([OrgUnit]) REFERENCES [dbo].[OrgUnit] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_OrgUnitUser_User]...';


GO
ALTER TABLE [dbo].[OrgUnitUser]
    ADD CONSTRAINT [FK_OrgUnitUser_User] FOREIGN KEY ([User]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatRoomChatFile_ChatRoomChat]...';


GO
ALTER TABLE [dbo].[ChatRoomChatFile]
    ADD CONSTRAINT [FK_ChatRoomChatFile_ChatRoomChat] FOREIGN KEY ([ChatRoomChat]) REFERENCES [dbo].[ChatRoomChat] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatRoomChatUserTagged_ChatRoomChat]...';


GO
ALTER TABLE [dbo].[ChatRoomChatUserTagged]
    ADD CONSTRAINT [FK_ChatRoomChatUserTagged_ChatRoomChat] FOREIGN KEY ([ChatRoomChat]) REFERENCES [dbo].[ChatRoomChat] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatRoomChatUserTagged_User]...';


GO
ALTER TABLE [dbo].[ChatRoomChatUserTagged]
    ADD CONSTRAINT [FK_ChatRoomChatUserTagged_User] FOREIGN KEY ([UserTagged]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatRoomChat_User]...';


GO
ALTER TABLE [dbo].[ChatRoomChat]
    ADD CONSTRAINT [FK_ChatRoomChat_User] FOREIGN KEY ([User]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatRoomChat_ChatRoom]...';


GO
ALTER TABLE [dbo].[ChatRoomChat]
    ADD CONSTRAINT [FK_ChatRoomChat_ChatRoom] FOREIGN KEY ([ChatRoom]) REFERENCES [dbo].[ChatRoom] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatRoomChat_Tenant]...';


GO
ALTER TABLE [dbo].[ChatRoomChat]
    ADD CONSTRAINT [FK_ChatRoomChat_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatRoom_Tenant]...';


GO
ALTER TABLE [dbo].[ChatRoom]
    ADD CONSTRAINT [FK_ChatRoom_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatRoom_User]...';


GO
ALTER TABLE [dbo].[ChatRoom]
    ADD CONSTRAINT [FK_ChatRoom_User] FOREIGN KEY ([UserCreator]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateQuery_Template]...';


GO
ALTER TABLE [dbo].[TemplateQuery]
    ADD CONSTRAINT [FK_TemplateQuery_Template] FOREIGN KEY ([Template]) REFERENCES [dbo].[Template] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateSection_Template]...';


GO
ALTER TABLE [dbo].[TemplateSection]
    ADD CONSTRAINT [FK_TemplateSection_Template] FOREIGN KEY ([Template]) REFERENCES [dbo].[Template] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateSection_Tenant]...';


GO
ALTER TABLE [dbo].[TemplateSection]
    ADD CONSTRAINT [FK_TemplateSection_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateFieldRelation_Tenant]...';


GO
ALTER TABLE [dbo].[TemplateFieldRelation]
    ADD CONSTRAINT [FK_TemplateFieldRelation_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateFieldRelation_TemplateFieldRelation]...';


GO
ALTER TABLE [dbo].[TemplateFieldRelation]
    ADD CONSTRAINT [FK_TemplateFieldRelation_TemplateFieldRelation] FOREIGN KEY ([TemplateFieldRelation]) REFERENCES [dbo].[TemplateField] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateFieldRelation_TemplateField]...';


GO
ALTER TABLE [dbo].[TemplateFieldRelation]
    ADD CONSTRAINT [FK_TemplateFieldRelation_TemplateField] FOREIGN KEY ([TemplateField]) REFERENCES [dbo].[TemplateField] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateFieldOption_TemplateField]...';


GO
ALTER TABLE [dbo].[TemplateFieldOption]
    ADD CONSTRAINT [FK_TemplateFieldOption_TemplateField] FOREIGN KEY ([TemplateField]) REFERENCES [dbo].[TemplateField] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateField_TemplateSection]...';


GO
ALTER TABLE [dbo].[TemplateField]
    ADD CONSTRAINT [FK_TemplateField_TemplateSection] FOREIGN KEY ([TemplateSection]) REFERENCES [dbo].[TemplateSection] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateField_Tenant]...';


GO
ALTER TABLE [dbo].[TemplateField]
    ADD CONSTRAINT [FK_TemplateField_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_Template_Tenant]...';


GO
ALTER TABLE [dbo].[Template]
    ADD CONSTRAINT [FK_Template_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_helptxt_help]...';


GO
ALTER TABLE [dbo].[helptxt]
    ADD CONSTRAINT [FK_helptxt_help] FOREIGN KEY ([help]) REFERENCES [dbo].[help] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_help_Language]...';


GO
ALTER TABLE [dbo].[help]
    ADD CONSTRAINT [FK_help_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_help_Tenant]...';


GO
ALTER TABLE [dbo].[help]
    ADD CONSTRAINT [FK_help_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatMessage_TenantId]...';


GO
ALTER TABLE [dbo].[ChatMessage]
    ADD CONSTRAINT [FK_ChatMessage_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatMessage_UserId]...';


GO
ALTER TABLE [dbo].[ChatMessage]
    ADD CONSTRAINT [FK_ChatMessage_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatMessage_FriendTenantId]...';


GO
ALTER TABLE [dbo].[ChatMessage]
    ADD CONSTRAINT [FK_ChatMessage_FriendTenantId] FOREIGN KEY ([FriendTenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ChatMessage_FriendUserId]...';


GO
ALTER TABLE [dbo].[ChatMessage]
    ADD CONSTRAINT [FK_ChatMessage_FriendUserId] FOREIGN KEY ([FriendUserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_Friendship_TenantId]...';


GO
ALTER TABLE [dbo].[Friendship]
    ADD CONSTRAINT [FK_Friendship_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_Friendship_UserId]...';


GO
ALTER TABLE [dbo].[Friendship]
    ADD CONSTRAINT [FK_Friendship_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_Friendship_FriendTenantId]...';


GO
ALTER TABLE [dbo].[Friendship]
    ADD CONSTRAINT [FK_Friendship_FriendTenantId] FOREIGN KEY ([FriendTenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_Friendship_FriendUserId]...';


GO
ALTER TABLE [dbo].[Friendship]
    ADD CONSTRAINT [FK_Friendship_FriendUserId] FOREIGN KEY ([FriendUserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_MailServiceMailAttach_MailServiceMailBody]...';


GO
ALTER TABLE [dbo].[MailServiceMailAttach]
    ADD CONSTRAINT [FK_MailServiceMailAttach_MailServiceMailBody] FOREIGN KEY ([MailServiceMailBody]) REFERENCES [dbo].[MailServiceMailBody] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_MailServiceMailBody_MailServiceMail]...';


GO
ALTER TABLE [dbo].[MailServiceMailBody]
    ADD CONSTRAINT [FK_MailServiceMailBody_MailServiceMail] FOREIGN KEY ([MailServiceMail]) REFERENCES [dbo].[MailServiceMail] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_MailServiceMailStatus_MailServiceMail]...';


GO
ALTER TABLE [dbo].[MailServiceMailStatus]
    ADD CONSTRAINT [FK_MailServiceMailStatus_MailServiceMail] FOREIGN KEY ([MailServiceMail]) REFERENCES [dbo].[MailServiceMail] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_MailServiceMailConfig_MailServiceMail]...';


GO
ALTER TABLE [dbo].[MailServiceMailConfig]
    ADD CONSTRAINT [FK_MailServiceMailConfig_MailServiceMail] FOREIGN KEY ([MailServiceMail]) REFERENCES [dbo].[MailServiceMail] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_MailServiceMail_MailServiceRequest]...';


GO
ALTER TABLE [dbo].[MailServiceMail]
    ADD CONSTRAINT [FK_MailServiceMail_MailServiceRequest] FOREIGN KEY ([TenantId], [MailServiceRequest]) REFERENCES [dbo].[MailServiceRequest] ([TenantId], [Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_MailServiceRequest_Tenant]...';


GO
ALTER TABLE [dbo].[MailServiceRequest]
    ADD CONSTRAINT [FK_MailServiceRequest_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_MailServiceRequest_User]...';


GO
ALTER TABLE [dbo].[MailServiceRequest]
    ADD CONSTRAINT [FK_MailServiceRequest_User] FOREIGN KEY ([UserCreator]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ToDoTimeSheet_Tenant]...';


GO
ALTER TABLE [dbo].[ToDoTimeSheet]
    ADD CONSTRAINT [FK_ToDoTimeSheet_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ToDoTimeSheet_ToDoActivity]...';


GO
ALTER TABLE [dbo].[ToDoTimeSheet]
    ADD CONSTRAINT [FK_ToDoTimeSheet_ToDoActivity] FOREIGN KEY ([ToDoActivity]) REFERENCES [dbo].[ToDoActivity] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ToDoTimeSheet_User]...';


GO
ALTER TABLE [dbo].[ToDoTimeSheet]
    ADD CONSTRAINT [FK_ToDoTimeSheet_User] FOREIGN KEY ([UserCreator]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateDefaultOUEditor_OrgUnit]...';


GO
ALTER TABLE [dbo].[TemplateDefaultOUEditor]
    ADD CONSTRAINT [FK_TemplateDefaultOUEditor_OrgUnit] FOREIGN KEY ([OrgUnit]) REFERENCES [dbo].[OrgUnit] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateDefaultOUEditor_Template]...';


GO
ALTER TABLE [dbo].[TemplateDefaultOUEditor]
    ADD CONSTRAINT [FK_TemplateDefaultOUEditor_Template] FOREIGN KEY ([Template]) REFERENCES [dbo].[Template] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateDefaultOUReader_OrgUnit]...';


GO
ALTER TABLE [dbo].[TemplateDefaultOUReader]
    ADD CONSTRAINT [FK_TemplateDefaultOUReader_OrgUnit] FOREIGN KEY ([OrgUnit]) REFERENCES [dbo].[OrgUnit] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateDefaultOUReader_Template]...';


GO
ALTER TABLE [dbo].[TemplateDefaultOUReader]
    ADD CONSTRAINT [FK_TemplateDefaultOUReader_Template] FOREIGN KEY ([Template]) REFERENCES [dbo].[Template] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateDefaultUserEditor_Template]...';


GO
ALTER TABLE [dbo].[TemplateDefaultUserEditor]
    ADD CONSTRAINT [FK_TemplateDefaultUserEditor_Template] FOREIGN KEY ([Template]) REFERENCES [dbo].[Template] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateDefaultUserEditor_User]...';


GO
ALTER TABLE [dbo].[TemplateDefaultUserEditor]
    ADD CONSTRAINT [FK_TemplateDefaultUserEditor_User] FOREIGN KEY ([User]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateDefaultUserReader_Template]...';


GO
ALTER TABLE [dbo].[TemplateDefaultUserReader]
    ADD CONSTRAINT [FK_TemplateDefaultUserReader_Template] FOREIGN KEY ([Template]) REFERENCES [dbo].[Template] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_TemplateDefaultUserReader_User]...';


GO
ALTER TABLE [dbo].[TemplateDefaultUserReader]
    ADD CONSTRAINT [FK_TemplateDefaultUserReader_User] FOREIGN KEY ([User]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ToDoActEvaluator_ToDoActivity]...';


GO
ALTER TABLE [dbo].[ToDoActEvaluator]
    ADD CONSTRAINT [FK_ToDoActEvaluator_ToDoActivity] FOREIGN KEY ([ToDoActivity]) REFERENCES [dbo].[ToDoActivity] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_ToDoActEvaluator_User]...';


GO
ALTER TABLE [dbo].[ToDoActEvaluator]
    ADD CONSTRAINT [FK_ToDoActEvaluator_User] FOREIGN KEY ([User]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_OrgUnit_OrgUnit]...';


GO
ALTER TABLE [dbo].[OrgUnit]
    ADD CONSTRAINT [FK_OrgUnit_OrgUnit] FOREIGN KEY ([ParentOU]) REFERENCES [dbo].[OrgUnit] ([Id]);


GO
PRINT N'Creando Clave externa [dbo].[FK_OrgUnit_Tenant]...';


GO
ALTER TABLE [dbo].[OrgUnit]
    ADD CONSTRAINT [FK_OrgUnit_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id]);


GO
PRINT N'Creando Vista [dbo].[OUUsersSecurity]...';


GO
CREATE VIEW [dbo].[OUUsersSecurity]
	AS SELECT SegOU.Id, SegOU.TenantId, SegOU.EfectiveId, SegOU.Name, SegOU.Level, OU.[User] FROM (
-- ***************** NIVEL 1
	select id, TenantId, id as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 1 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, ParentOU as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 2 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select ParentOU from OrgUnit OU2 where level = 2
		and OU3.ParentOU = OU2.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU3
	where Level = 3 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select 
			(Select ParentOU from OrgUnit OU2 where level = 2
			and OU3.ParentOU = OU2.Id)
		 from OrgUnit OU3 where level = 3
		and OU4.ParentOU = OU3.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU4
	where Level = 4 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select 
			(Select 
				(Select ParentOU from OrgUnit OU2 where level = 2
				and OU3.ParentOU = OU2.Id)
			 from OrgUnit OU3 where level = 3
			and OU4.ParentOU = OU3.Id)
		 from OrgUnit OU4 where level = 4
		and OU5.ParentOU = OU4.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU5
	where Level = 5 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select 
			(Select 
				(Select 
					(Select ParentOU from OrgUnit OU2 where level = 2
					and OU3.ParentOU = OU2.Id)
				 from OrgUnit OU3 where level = 3
				and OU4.ParentOU = OU3.Id)
			 from OrgUnit OU4 where level = 4
			and OU5.ParentOU = OU4.Id)
		 from OrgUnit OU5 where level = 5
		and OU6.ParentOU = OU5.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU6
	where Level = 6 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select 
			(Select 
				(Select 
					(Select 
						(Select ParentOU from OrgUnit OU2 where level = 2
						and OU3.ParentOU = OU2.Id)
					 from OrgUnit OU3 where level = 3
					and OU4.ParentOU = OU3.Id)
				 from OrgUnit OU4 where level = 4
				and OU5.ParentOU = OU4.Id)
			 from OrgUnit OU5 where level = 5
			and OU6.ParentOU = OU5.Id)
		 from OrgUnit OU6 where level = 6
		and OU7.ParentOU = OU6.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU7
	where Level = 7 AND (IsDeleted IS NULL OR IsDeleted = 0)
	-- ***************** NIVEL 2
	UNION
	select id, TenantId, id as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 2 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, ParentOU as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 3 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select ParentOU from OrgUnit OU3 where level = 3
		and OU4.ParentOU = OU3.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU4
	where Level = 4 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select 
			(Select ParentOU from OrgUnit OU3 where level = 3
			and OU4.ParentOU = OU3.Id)
		 from OrgUnit OU4 where level = 4
		and OU5.ParentOU = OU4.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU5
	where Level = 5 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select 
			(Select 
				(Select ParentOU from OrgUnit OU3 where level = 3
				and OU4.ParentOU = OU3.Id)
			 from OrgUnit OU4 where level = 4
			and OU5.ParentOU = OU4.Id)
		 from OrgUnit OU5 where level = 5
		and OU6.ParentOU = OU5.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU6
	where Level = 6 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select 
			(Select 
				(Select 
					(Select ParentOU from OrgUnit OU3 where level = 3
					and OU4.ParentOU = OU3.Id)
				 from OrgUnit OU4 where level = 4
				and OU5.ParentOU = OU4.Id)
			 from OrgUnit OU5 where level = 5
			and OU6.ParentOU = OU5.Id)
		 from OrgUnit OU6 where level = 6
		and OU7.ParentOU = OU6.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU7
	where Level = 7 AND (IsDeleted IS NULL OR IsDeleted = 0)
	-- ***************** NIVEL 3
	UNION
	select id, TenantId, id as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 3 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, ParentOU as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 4 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select ParentOU from OrgUnit OU4 where level = 4
		and OU5.ParentOU = OU4.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU5
	where Level = 5 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select 
			(Select ParentOU from OrgUnit OU4 where level = 4
			and OU5.ParentOU = OU4.Id)
		 from OrgUnit OU5 where level = 5
		and OU6.ParentOU = OU5.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU6
	where Level = 6 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select 
			(Select 
				(Select ParentOU from OrgUnit OU4 where level = 4
				and OU5.ParentOU = OU4.Id)
			 from OrgUnit OU5 where level = 5
			and OU6.ParentOU = OU5.Id)
		 from OrgUnit OU6 where level = 6
		and OU7.ParentOU = OU6.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU7
	where Level = 7 AND (IsDeleted IS NULL OR IsDeleted = 0)
	-- ***************** NIVEL 4
	UNION
	select id, TenantId, id as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 4 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, ParentOU as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 5 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select ParentOU from OrgUnit OU5 where level = 5
		and OU6.ParentOU = OU5.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU6
	where Level = 6 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select 
			(Select ParentOU from OrgUnit OU5 where level = 5
			and OU6.ParentOU = OU5.Id)
		 from OrgUnit OU6 where level = 6
		and OU7.ParentOU = OU6.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU7
	where Level = 7 AND (IsDeleted IS NULL OR IsDeleted = 0)
	-- ***************** NIVEL 5
	UNION
	select id, TenantId, id as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 5 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, ParentOU as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 6 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, 
		(Select ParentOU from OrgUnit OU6 where level = 6
		and OU7.ParentOU = OU6.Id) as 'EfectiveId', Name, Level
	from OrgUnit OU7
	where Level = 7 AND (IsDeleted IS NULL OR IsDeleted = 0)
	-- ***************** NIVEL 6
	UNION
	select id, TenantId, id as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 6 AND (IsDeleted IS NULL OR IsDeleted = 0)
	UNION
	select id, TenantId, ParentOU as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 7 AND (IsDeleted IS NULL OR IsDeleted = 0)
	-- ***************** NIVEL 7
	UNION
	select id, TenantId, id as 'EfectiveId', Name, Level from OrgUnit 
	where Level = 7 AND (IsDeleted IS NULL OR IsDeleted = 0)
) as SegOU
INNER JOIN OrgUnitUser OU ON SegOU.EfectiveId = OU.OrgUnit;
GO
PRINT N'Creando Procedimiento [dbo].[spr_obtienedatosporseccion]...';


GO
CREATE PROCEDURE [dbo].[spr_obtienedatosporseccion]
    @strSelectTabla NVARCHAR(MAX), 
	@numeropagina int = 0, 
	@porpagina int = 10, 
	@orderby NVARCHAR(MAX) = NULL
AS
   DECLARE @strSQL NVARCHAR(MAX)

   IF (@orderby IS NULL OR NOT (LEN(@orderby) > 0))
    SET @orderby = 'Id ASC'

   set @strSQL = 'declare @total int '

   set @strSQL = @strSQL + ' SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; '
   
   set @strSQL = @strSQL + ' select @total = COUNT(*) from ( '  + @strSelectTabla + ' ) as tablaprincipal '
   
   set @strSQL = @strSQL + 'SELECT *, @total as total FROM ( ' +     
                             ' SELECT *, ROW_NUMBER() OVER (order by ' + rtrim(@orderby) + ') AS row ' +
                             ' FROM( ' + @strSelectTabla + ' ) alias1 ' +
                             ' ) alias2 ' +
                             ' WHERE 1 = 1 ' +
                             ' and row > ' + convert(char(10),(@numeropagina * @porpagina)) +
							 ' and row <= ' + convert(char(10),((@numeropagina * @porpagina) + @porpagina)) 
  
    EXEC sp_executesql @strSQL
GO
PRINT N'Creando Procedimiento [dbo].[AuditLog_AddEntry_Other]...';


GO
CREATE PROCEDURE [dbo].[AuditLog_AddEntry_Other] (
	@machineName nvarchar(200),
	@logged datetime,
	@level varchar(5),
	@message nvarchar(max),
	@logger nvarchar(300),
	@properties nvarchar(max),
	@callsite nvarchar(300),
	@exception nvarchar(max)
) AS
BEGIN

	DECLARE @Severity TINYINT; -- Trace = 0, Debug = 1, Info = 2, Warn = 3, Error = 4, Fatal = 5

	IF UPPER(@level) = 'TRACE'
		SET @Severity = 0;
	
	IF UPPER(@level) = 'DEBUG'
		SET @Severity = 1;

	IF UPPER(@level) = 'INFO'
		SET @Severity = 2;

	IF UPPER(@level) = 'WARN'
		SET @Severity = 3;

	IF UPPER(@level) = 'ERROR'
		SET @Severity = 4;

	IF UPPER(@level) = 'FATAL'
		SET @Severity = 5;

	IF @logger = ''
		SET @logger = NULL;

	IF @callsite = ''
		SET @callsite = NULL;

	IF @properties = ''
		SET @properties = NULL;

	IF @logged = ''
		SET @logged = NULL;

	IF @machineName = ''
		SET @machineName = NULL;

	IF @exception = ''
		SET @exception = NULL;

	INSERT INTO [dbo].[AuditLog] (
		[TenantId],
		[UserId],
		[ServiceName],
		[MethodName],
		[Parameters],
		[ExecutionDatetime],
		[ExecutionDuration],
		[ClientIpAddress],
		[ClientName],
		[BroserInfo],
		[Exception],
		[Severity]
	) VALUES (
		NULL,
		NULL,
		@logger,
		@callsite,
		@properties,
		@logged,
		NULL,
		NULL,
		@machineName,
		NULL,
		@exception,
		@Severity
	);

END
GO
PRINT N'Creando Procedimiento [dbo].[AuditLog_AddEntry]...';


GO
CREATE PROCEDURE [dbo].[AuditLog_AddEntry] (
	@TenantId INT,
	@UserId BIGINT,
    @ImpersonalizerUserId BIGINT,
	@ServiceName VARCHAR(250),
	@MethodName VARCHAR(250),
	@Parameters NVARCHAR(MAX),
	@ExecutionDatetime DATETIME,
	@ExecutionDuration INT,
	@ClientIpAddress VARCHAR(64),
	@ClientName VARCHAR(128),
	@BroserInfo VARCHAR(250),
	@Exception VARCHAR(MAX),
    @CustomData NVARCHAR(MAX),
	@Severity TINYINT
) AS
BEGIN
	
	IF @TenantId = ''
		SET @TenantId = NULL;

	IF @UserId = ''
		SET @UserId = NULL;

    IF @ImpersonalizerUserId = ''
		SET @ImpersonalizerUserId = NULL;

	IF @ServiceName = ''
		SET @ServiceName = NULL;

	IF @MethodName = ''
		SET @MethodName = NULL;

	IF @ClientIpAddress = ''
		SET @ClientIpAddress = NULL;

	IF @ClientName = ''
		SET @ClientName = NULL;

	IF @BroserInfo = ''
		SET @BroserInfo = NULL;

	IF @Exception = ''
		SET @Exception = NULL;

	INSERT INTO [dbo].[AuditLog] (
		[TenantId],
		[UserId],
        [ImpersonalizerUserId],
		[ServiceName],
		[MethodName],
		[Parameters],
		[ExecutionDatetime],
		[ExecutionDuration],
		[ClientIpAddress],
		[ClientName],
		[BroserInfo],
		[Exception],
        [CustomData],
		[Severity]
	) VALUES (
		@TenantId,
		@UserId,
        @ImpersonalizerUserId,
		@ServiceName,
		@MethodName,
		@Parameters,
		@ExecutionDatetime,
		@ExecutionDuration,
		@ClientIpAddress,
		@ClientName,
		@BroserInfo,
		@Exception,
        @CustomData,
		@Severity
	);
END

GO
PRINT N'Creando Función [dbo].[AtTimeZone]...';

GO
CREATE FUNCTION [dbo].[AtTimeZone](@dt DATETIME, @tz VARCHAR(MAX)) RETURNS DATETIME AS
BEGIN
	IF @dt IS NULL
		RETURN NULL;

	RETURN DATEADD(MINUTE, DATEPART(TZOFFSET, @dt AT TIME ZONE @tz), @dt)
END

GO
PRINT N'Creando Función [dbo].[CreateDateTime]...';

GO
CREATE FUNCTION [dbo].[CreateDateTime](@dt DATE, @ts TIME(7)) RETURNS DATETIME AS
BEGIN
	RETURN CAST(@dt AS DATETIME) + CAST(@ts AS DATETIME);
END
GO

GO
PRINT N'Creando Función [dbo].[DateDiffCustom]...';

GO
-- =============================================
-- Author:		Fernando Castro
-- Create date: 22-04-22
-- Description:	Retorna la diferencia entre dos fechas en la unidad especificada
-- =============================================
CREATE FUNCTION [dbo].[DateDiffCustom]
(
	@unit VARCHAR(15),
	@dt1 DateTime,
	@dt2 DateTime
)
RETURNS INT
AS
BEGIN
	DECLARE @result AS INT;

	SET @result = CASE UPPER(TRIM(@unit))
		WHEN 'YEAR' THEN DATEDIFF(YEAR, @dt1, @dt2)
		WHEN 'MONTH' THEN DATEDIFF(MONTH, @dt1, @dt2)
		WHEN 'DAY' THEN DATEDIFF(DAY, @dt1, @dt2)
		WHEN 'HOUR' THEN DATEDIFF(HOUR, @dt1, @dt2)
		WHEN 'MINUTE' THEN DATEDIFF(MINUTE, @dt1, @dt2)
		WHEN 'SECOND' THEN DATEDIFF(SECOND, @dt1, @dt2)
		WHEN 'MILLISECOND' THEN DATEDIFF(MILLISECOND, @dt1, @dt2)
	END;

	RETURN @result;
END
GO



/*
Plantilla de script posterior a la implementación                            
--------------------------------------------------------------------------------------
 Este archivo contiene instrucciones de SQL que se anexarán al script de compilación.        
 Use la sintaxis de SQLCMD para incluir un archivo en el script posterior a la implementación.            
 Ejemplo:      :r .\miArchivo.sql                                
 Use la sintaxis de SQLCMD para hacer referencia a una variable en el script posterior a la implementación.        
 Ejemplo:      :setvar TableName miTabla                            
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS (SELECT 1
           FROM   [__EFMigrationsHistory]
           WHERE  [MigrationId] = '20181203174231_InitialCreate')
BEGIN

DECLARE @admonName VARCHAR(13) = 'Administrador';
DECLARE @strDefault VARCHAR(7) = 'Default';
DECLARE @strFalse VARCHAR(5) = 'false';

SET IDENTITY_INSERT [User] ON;  

INSERT INTO [User] ([Id], [TenantId], [UserLogin], [Password], [Name], [Lastname], [SecondLastname], [EmailAddress], [IsEmailConfirmed], [PhoneNumber]
    ,[IsPhoneConfirmed], [CreationTime], [ChangePassword], [AccessFailedCount], [ProfilePictureId], [UserLocked], [IsLockoutEnabled], [IsActive], [IsDeleted])
VALUES
    (1, NULL, 'admin', 'AQAAAAEAACcQAAAAECqqu++Q+dyd14CAgQ09k26e/abCYTIm/VjrVN0ScQSPs7896bkb4dBTgbgE7ZW82A==', @admonName, 'Host', NULL, 'user@correo.com.mx'
    , 1, NULL, 0, GETDATE(), 1, 0, NULL, 0, 1, 1, 0);

SET IDENTITY_INSERT [User] OFF;  

SET IDENTITY_INSERT [Tenant] ON;  

INSERT INTO [Tenant] ([Id], [TenancyName], [Name], [CreationTime], [IsActive], [IsDeleted])
VALUES (1, @strDefault, @strDefault, GETDATE(), 1, 0);

SET IDENTITY_INSERT [Tenant] OFF;  

SET IDENTITY_INSERT [User] ON; 

INSERT INTO [User] ([Id], [TenantId], [UserLogin], [Password], [Name], [Lastname], [SecondLastname], [EmailAddress], [IsEmailConfirmed], [PhoneNumber]
    ,[IsPhoneConfirmed], [CreationTime], [ChangePassword], [AccessFailedCount], [ProfilePictureId], [UserLocked], [IsLockoutEnabled], [IsActive], [IsDeleted])
VALUES
    (2, 1, 'admin', 'AQAAAAEAACcQAAAAECqqu++Q+dyd14CAgQ09k26e/abCYTIm/VjrVN0ScQSPs7896bkb4dBTgbgE7ZW82A==', @admonName, 'Tenant', @strDefault, 'user@algoria.com.mx'
    , 1, NULL, 0, GETDATE(), 1, 0, NULL, 0, 1, 1, 0);

SET IDENTITY_INSERT [User] OFF;  

SET IDENTITY_INSERT [Language] ON;  

INSERT INTO [Language] ([Id], [TenantId], [Name], [DisplayName], [IsActive])
VALUES (1, NULL, 'es-MX', 'Español (México)', 1);

INSERT INTO [Language] ([Id], [TenantId], [Name], [DisplayName], [IsActive])
VALUES (2, 1, 'es-MX', 'Español (México)', 1);

SET IDENTITY_INSERT [Language] OFF; 

SET IDENTITY_INSERT [Role] ON;  

INSERT INTO [Role] ([Id], [TenantId], [Name], [DisplayName], [IsActive])
VALUES (1, NULL, 'Admin', @admonName, 1);

INSERT INTO [Role] ([Id], [TenantId], [Name], [DisplayName], [IsActive])
VALUES (2, 1, 'Admin', @admonName, 1);

SET IDENTITY_INSERT [Role] OFF; 

INSERT INTO [Permission] ([Role], [Name], [IsGranted]) VALUES 
(1, 'Pages', 1),
(1, 'Pages.Administration', 1),
(1, 'Pages.Administration.Roles', 1),
(1, 'Pages.Administration.Roles.Create', 1),
(1, 'Pages.Administration.Roles.Edit', 1),
(1, 'Pages.Administration.Roles.Delete', 1),
(1, 'Pages.Administration.Users', 1),
(1, 'Pages.Administration.Users.Create', 1),
(1, 'Pages.Administration.Users.Edit', 1),
(1, 'Pages.Administration.Users.Delete', 1),
(1, 'Pages.Administration.Users.ChangePermissions', 1),
(1, 'Pages.Administration.Users.Impersonation', 1),
(1, 'Pages.Administration.Languages', 1),
(1, 'Pages.Administration.Languages.Create', 1),
(1, 'Pages.Administration.Languages.Edit', 1),
(1, 'Pages.Administration.Languages.Delete', 1),
(1, 'Pages.Administration.Languages.ChangeTexts', 1),
(1, 'Pages.Administration.AuditLogs', 1),
(1, 'Pages.Administration.Host.Settings', 1),
(1, 'Pages.Administration.Host.Maintenance', 1),
(1, 'Pages.Tenants', 1),
(1, 'Pages.Tenants.Create', 1),
(1, 'Pages.Tenants.Edit', 1),
(1, 'Pages.Tenants.Delete', 1),
(1, 'Pages.Tenants.Impersonation', 1),
(1, 'hosthost.1', 1),
(1, 'conftcor.0', 1),
(1, 'conftcor.1', 1),
(1, 'conftcor.2', 1),
(1, 'conftcor.3', 1),
(1, 'conftcor.4', 1),
(1, 'conftcor.5', 1),
(1, 'conftcor.6', 1),
(2, 'Pages', 1),
(2, 'Pages.Administration', 1),
(2, 'Pages.Administration.Roles', 1),
(2, 'Pages.Administration.Roles.Create', 1),
(2, 'Pages.Administration.Roles.Edit', 1),
(2, 'Pages.Administration.Roles.Delete', 1),
(2, 'Pages.Administration.Users', 1),
(2, 'Pages.Administration.Users.Create', 1),
(2, 'Pages.Administration.Users.Edit', 1),
(2, 'Pages.Administration.Users.Delete', 1),
(2, 'Pages.Administration.Users.ChangePermissions', 1),
(2, 'Pages.Administration.Users.Impersonation', 1),
(2, 'Pages.Administration.Languages', 1),
(2, 'Pages.Administration.Languages.Create', 1),
(2, 'Pages.Administration.Languages.Edit', 1),
(2, 'Pages.Administration.Languages.Delete', 1),
(2, 'Pages.Administration.Languages.ChangeTexts', 1),
(2, 'Pages.Administration.AuditLogs', 1),
(2, 'Pages.Administration.Tenant.Settings', 1),
(2, 'Pages.Tenant.Dashboard', 1),
(2, 'Pages.Tenant.Catalogos', 1),
(2, 'conftcor.0', 1),
(2, 'conftcor.1', 1),
(2, 'conftcor.2', 1),
(2, 'conftcor.3', 1),
(2, 'conftcor.4', 1),
(2, 'conftcor.5', 1),
(2, 'conftcor.6', 1);

SET IDENTITY_INSERT [UserRole] ON;  

INSERT INTO [UserRole] ([Id], [UserId], [RoleId])
VALUES (1, 1, 1);

INSERT INTO [UserRole] ([Id], [UserId], [RoleId])
VALUES (2, 2, 2);

SET IDENTITY_INSERT [UserRole] OFF;  

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'LanguageDefault', '1');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'WebSiteRootAddress', 'http://localhost:8089/');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'EnableUserBlocking', @strFalse);

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'FailedAttemptsToBlockUser', '5');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'UserBlockingDuration', '300');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'EnableTwoFactorLogin', @strFalse);

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'EnableMailVerification', @strFalse);

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'EnableSMSVerification', @strFalse);

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'EnableBrowserRemenberMe', @strFalse);

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'MailEnableSSL', @strFalse);

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'MailUseDefaultCredentials', 'true');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (NULL, 'MailGroup', '1');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (1, 'LanguageDefault', '2');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (1, 'EnableUserBlocking', @strFalse);

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (1, 'FailedAttemptsToBlockUser', '5');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (1, 'UserBlockingDuration', '300');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (1, 'EnablePasswordPeriod', @strFalse);

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (1, 'PasswordValidDays', '30');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (1, 'MailEnableSSL', @strFalse);

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (1, 'MailUseDefaultCredentials', 'true');

INSERT INTO [Setting] ([TenantId], [Name], [value])
VALUES (1, 'MailGroup', '2');

SET IDENTITY_INSERT [mailgroup] ON;  

INSERT INTO [mailgroup] ([Id], [TenantId], [DisplayName])
VALUES (1, NULL, 'Tema de correos default');

INSERT INTO [mailgroup] ([Id], [TenantId], [DisplayName])
VALUES (2, 1, 'Tema de correos default');

SET IDENTITY_INSERT [mailgroup] OFF;  

INSERT INTO [mailtemplate] ([TenantId], [mailgroup], [mailkey], [DisplayName], [Subject], [IsActive])
VALUES (NULL, 1, 'usr-nuevo', 'Plantilla Nuevo usuario', 'Nuevo Usuario', 1);

INSERT INTO [mailtemplate] ([TenantId], [mailgroup], [mailkey], [DisplayName], [Subject], [IsActive])
VALUES (NULL, 1, 'usr-reset', 'Plantilla Reset Contraseña por Usuario', 'Reset de Contraseña por usuario', 1);

INSERT INTO [mailtemplate] ([TenantId], [mailgroup], [mailkey], [DisplayName], [Subject], [IsActive])
VALUES (NULL, 1, 'usr-unblock', 'Plantilla Desbloquear Usuario', 'Desbloquear Usuario', 1);

INSERT INTO [mailtemplate] ([TenantId], [mailgroup], [mailkey], [DisplayName], [Subject], [IsActive])
VALUES (NULL, 1, 'usr-modi', 'Plantilla Modificar Contraseña por Administrador', 'Modificar Contraseña por Administrador', 1);

INSERT INTO [mailtemplate] ([TenantId], [mailgroup], [mailkey], [DisplayName], [Subject], [IsActive])
VALUES (NULL, 1, 'tenant-reg', 'Plantilla Registrar tenant', 'Registrar tenant', 1);

INSERT INTO [mailtemplate] ([TenantId], [mailgroup], [mailkey], [DisplayName], [Subject], [IsActive])
VALUES (1, 2, 'usr-nuevo', 'Plantilla Nuevo usuario', 'Nuevo Usuario', 1);

INSERT INTO [mailtemplate] ([TenantId], [mailgroup], [mailkey], [DisplayName], [Subject], [IsActive])
VALUES (1, 2, 'usr-reset', 'Plantilla Reset Contraseña por Usuario', 'Reset de Contraseña por usuario', 1);

INSERT INTO [mailtemplate] ([TenantId], [mailgroup], [mailkey], [DisplayName], [Subject], [IsActive])
VALUES (1, 2, 'usr-unblock', 'Plantilla Desbloquear Usuario', 'Desbloquear Usuario', 1);

INSERT INTO [mailtemplate] ([TenantId], [mailgroup], [mailkey], [DisplayName], [Subject], [IsActive])
VALUES (1, 2, 'usr-modi', 'Plantilla Modificar Contraseña por Administrador', 'Modificar Contraseña por Administrador', 1);

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES ('20181203174231_InitialCreate', 'CORE(181203)')

END

GO
PRINT N'Actualización completada.';


GO
