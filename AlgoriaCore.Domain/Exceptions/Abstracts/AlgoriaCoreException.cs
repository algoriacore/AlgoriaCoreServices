using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions.Abstracts
{
    public abstract class AlgoriaCoreException : Exception
    {
        public string ErrorCode { private set; get; }
        public LogLevel LogLevel { private set; get; }

        protected AlgoriaCoreException()
        {
            ErrorCode = AlgoriaCoreExceptionErrorCodes.GeneralError;
            LogLevel = LogLevel.None;
        }

		protected AlgoriaCoreException(string message)
            :base(message)
        {
            ErrorCode = AlgoriaCoreExceptionErrorCodes.GeneralError;
            LogLevel = LogLevel.None;
        }

		protected AlgoriaCoreException(string errorCode, string message)
            :base(message)
        {
            ErrorCode = errorCode;
            LogLevel = LogLevel.None;
        }

		protected AlgoriaCoreException(string errorCode, string message, LogLevel logLevel) : this(errorCode, message)
        {
            LogLevel = logLevel;
        }

		// Without this constructor, deserialization will fail
		protected AlgoriaCoreException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
