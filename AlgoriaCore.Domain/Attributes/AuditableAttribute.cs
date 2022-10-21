using System;

namespace AlgoriaCore.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class AuditableAttribute : Attribute
    {
        public AuditableAttribute(bool isAuditable)
        {
            IsAuditable = isAuditable;
        }

        public bool IsAuditable
        {
            get;
        }
    }
}
