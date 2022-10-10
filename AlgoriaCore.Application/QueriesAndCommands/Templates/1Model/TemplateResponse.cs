using System;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateResponse
    {
        public long? Id { get; set; }
        public string RGBColor { get; set; }
        public string NameSingular { get; set; }
        public string NamePlural { get; set; }
        public string Description { get; set; }
        public Guid? Icon { get; set; }
        public string TableName { get; set; }
        public bool IsTableGenerated { get; set; }
        public bool HasChatRoom { get; set; }
        public bool IsActivity { get; set; }
        public bool HasSecurity { get; set; }
        public bool IsActive { get; set; }
    }
}