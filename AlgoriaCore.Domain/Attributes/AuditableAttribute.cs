using System;

namespace AlgoriaCore.Domain.Attributes
{
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
