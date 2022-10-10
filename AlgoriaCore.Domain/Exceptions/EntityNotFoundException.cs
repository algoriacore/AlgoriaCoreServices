using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : AlgoriaErrorException
    {
        public EntityNotFoundException(string entityName)
            : base(AlgoriaCoreExceptionErrorCodes.EntityNotFound, entityName)
        {
        }

		protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
