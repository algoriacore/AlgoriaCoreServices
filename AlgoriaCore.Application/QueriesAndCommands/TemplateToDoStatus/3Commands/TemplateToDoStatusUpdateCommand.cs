using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusUpdateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public TemplateToDoStatusType Type { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}