using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    //Esta excepción ocurre únicamente durante el login de usuarios.
    //Indica que un usuario no tiene credenciales válidas
    [Serializable]
    public class NoValidUserException : AlgoriaErrorException
    {
        public NoValidUserException(string message)
            : base(AlgoriaCoreExceptionErrorCodes.UserNoValid, message)
        {
        }

		protected NoValidUserException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
