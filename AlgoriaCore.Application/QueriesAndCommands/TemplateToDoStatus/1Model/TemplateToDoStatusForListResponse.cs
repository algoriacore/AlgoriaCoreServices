using AlgoriaCore.Application.Managers.Templates.Dto;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusForListResponse
    {
        public long Id { get; set; }
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
}