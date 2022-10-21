using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
    public class EntityHasRelationshipsException : AlgoriaErrorException
    {
        public EntityHasRelationshipsException(string message)
            : base(AlgoriaCoreExceptionErrorCodes.RecordHasRelationships, message)
        {
        }

		protected EntityHasRelationshipsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
