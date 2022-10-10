using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusCreateCommand : IRequest<long>
    {
        public long Template { get; set; }
        public TemplateToDoStatusType Type { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}