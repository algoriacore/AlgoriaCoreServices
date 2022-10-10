using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldCreateCommand : IRequest<long>
    {
        public long? TemplateSection { get; set; }
        public string Name { get; set; }
        public string FieldName { get; set; }
        public TemplateFieldType? FieldType { get; set; }
        public short? FieldSize { get; set; }
        public TemplateFieldControl? FieldControl { get; set; }
        public string InputMask { get; set; }
        public bool HasKeyFilter { get; set; }
        public string KeyFilter { get; set; }
        public bool IsRequired { get; set; }
        public bool ShowOnGrid { get; set; }
        public short? Order { get; set; }
        public bool InheritSecurity { get; set; }

        public long? TemplateFieldRelationTemplateField { get; set; }
        public List<TemplateFieldOptionDto> Options { get; set; }
    }
}