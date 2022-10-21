using System;

namespace AlgoriaCore.Domain.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public class MongoTransactionalAttribute : Attribute
    {
    }
}
