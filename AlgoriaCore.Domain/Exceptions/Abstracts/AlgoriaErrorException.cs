using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions.Abstracts
{
    public abstract class AlgoriaErrorException : AlgoriaCoreException
    {
        protected AlgoriaErrorException(string message) : this(AlgoriaCoreExceptionErrorCodes.GeneralError, message)
        {
        }

        protected AlgoriaErrorException(string errorCode, string message) : base(errorCode, message, Microsoft.Extensions.Logging.LogLevel.Error)
        {
        }

		// Agregar constructor con la siguiente firma:
		protected AlgoriaErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
