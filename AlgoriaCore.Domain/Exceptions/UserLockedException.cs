using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
    public class UserLockedException : AlgoriaErrorException
    {
        public UserLockedException(string message)
            : base(AlgoriaCoreExceptionErrorCodes.UserLocked, message)
        {
        }

		protected UserLockedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
