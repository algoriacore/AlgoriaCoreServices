using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
    public class WrongUserNameOrPasswordException : AlgoriaErrorException
    {
        public WrongUserNameOrPasswordException(string message) : base(message)
        {
        }

		protected WrongUserNameOrPasswordException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
