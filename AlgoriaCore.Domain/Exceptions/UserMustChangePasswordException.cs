using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
    public class UserMustChangePasswordException : AlgoriaErrorException
    {
        public UserMustChangePasswordException(string message)
            : base(AlgoriaCoreExceptionErrorCodes.UserMustChangePassword, message)
        {
        }

		protected UserMustChangePasswordException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
