using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
    //Indica que una operación obligatoriamente se debe ejecutar en modo HOST
    public class NoHostException : AlgoriaErrorException
    {
        public NoHostException(string message)
            : base(AlgoriaCoreExceptionErrorCodes.HostMode, message)
        {
        }

		protected NoHostException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
