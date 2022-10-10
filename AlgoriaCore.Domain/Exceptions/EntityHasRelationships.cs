using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
    public class EntityHasRelationships : AlgoriaErrorException
    {
        public EntityHasRelationships(string message)
            : base(AlgoriaCoreExceptionErrorCodes.RecordHasRelationships, message)
        {
        }

		protected EntityHasRelationships(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
