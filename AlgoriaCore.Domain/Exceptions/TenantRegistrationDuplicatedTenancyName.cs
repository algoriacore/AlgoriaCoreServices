using AlgoriaCore.Domain.Exceptions.Abstracts;
using System;
using System.Runtime.Serialization;

namespace AlgoriaCore.Domain.Exceptions
{
    [Serializable]
    public class TenantRegistrationDuplicatedTenancyNameException : AlgoriaErrorException
    {
        public TenantRegistrationDuplicatedTenancyNameException(string message)
            :base(AlgoriaCoreExceptionErrorCodes.TenantDuplicatedTenancyName, message)
        {

        }

		protected TenantRegistrationDuplicatedTenancyNameException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
