using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
    public class UserUnauthorizedException : AlgoriaErrorException
    {
		//Esta excepción ocurre cuando el usuario no tiene acceso a algún recurso
		public UserUnauthorizedException()
            : base(AlgoriaCoreExceptionErrorCodes.UserUnauthorized)
        {
        }

		protected UserUnauthorizedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
