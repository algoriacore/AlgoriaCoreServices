using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
    public class EntityDuplicatedException : AlgoriaErrorException
    {
        public EntityDuplicatedException(string message)
            : base(AlgoriaCoreExceptionErrorCodes.EntityDuplicated, message)
        {
        }

		protected EntityDuplicatedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
