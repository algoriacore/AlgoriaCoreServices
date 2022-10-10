using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions.Abstracts
{
    [Serializable]
    public abstract class AlgoriaCriticalException : AlgoriaCoreException
    {
        protected AlgoriaCriticalException(string errorCode, string message) : base(errorCode, message, Microsoft.Extensions.Logging.LogLevel.Critical)
        {

        }

		protected AlgoriaCriticalException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
