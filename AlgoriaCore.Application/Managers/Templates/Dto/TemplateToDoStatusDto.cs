using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.Templates.Dto
{
    public class TemplateToDoStatusDto
    {
        public long? Id { get; set; }
        public long Template { get; set; }
        public string TemplateDesc { get; set; }
        public TemplateToDoStatusType Type { get; set; }
        public string TypeDesc { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string IsDefaultDesc { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
    }

    public enum TemplateToDoStatusType : byte
    {
        Pending = 1,
        InRevision = 2,
        Returned = 3,
        Rejected = 4,
        Closed = 5,
        Canceled = 9
    }
}
