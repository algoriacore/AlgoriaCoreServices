using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions.Abstracts
{
    [Serializable]
    public abstract class AlgoriaWarningException : AlgoriaCoreException
    {
        protected AlgoriaWarningException(string errorCode, string message) : base(errorCode, message, Microsoft.Extensions.Logging.LogLevel.Warning)
        {

        }

		protected AlgoriaWarningException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
