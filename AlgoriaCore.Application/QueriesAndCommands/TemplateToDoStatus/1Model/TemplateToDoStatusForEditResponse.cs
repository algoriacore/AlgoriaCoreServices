using AlgoriaCore.Application.Managers.Templates.Dto;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusForEditResponse
    {
        public long? Id { get; set; }
        public long Template { get; set; }
        public TemplateToDoStatusType Type { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}