using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
	public class AlgoriaCoreGeneralException : AlgoriaErrorException
	{
		public AlgoriaCoreGeneralException(string message) : base(message)
		{
		}

		protected AlgoriaCoreGeneralException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
