namespace AlgoriaCore.Domain.Email
{
    public static class EmailVariables
	{
		public static class TenantRegistration
		{
			public const string TenantShortName = "TenantShortName";
			public const string TenantName = "TenantName";
			public const string Name = "Name";
			public const string LastName = "LastName";
			public const string SecondLastName = "SecondLastName";
			public const string Email = "Email";
			public const string ConfirmationCode = "ConfirmationCode";
			public const string ActivationUrl = "ActivationUrl";
		}

		public static class ResetPassword
		{
			public const string UserName = "UserName";
			public const string Name = "Name";
			public const string LastName = "LastName";
			public const string SecondLastName = "SecondLastName";
			public const string FullName = "FullName";
			public const string Email = "Email";
			public const string ConfirmationCode = "ConfirmationCode";
			public const string ResetUrl = "ResetUrl";
		}

		public static class NewUser
		{
			public const string UserName = "UserName";
			public const string FullName = "FullName";
			public const string Password = "Password";
		}

		public static class ChangePassword
		{
			public const string FullName = "FullName";
		}

		public static class UnblockUser
		{
			public const string FullName = "FullName";
		}

		public static class ModifyUser
		{
			public const string FullName = "FullName";
		}

		public static class TagChatUser
		{
			public const string ChatRoom = "ChatRoom";
			public const string ChatRoomDescription = "ChatRoomDescription";
			public const string TaggedUser = "TaggedUser";
			public const string TaggedBy = "TaggedBy";
			public const string TaggedDateTime = "TaggedDateTime";
			public const string Comment = "Comment";
		}
	}
}
