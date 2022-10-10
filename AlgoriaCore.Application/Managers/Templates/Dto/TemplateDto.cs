using System;

namespace AlgoriaCore.Application.Managers.Templates.Dto
{
    public class TemplateDto
    {
        public long? Id { get; set; }
        public string RGBColor { get; set; }
        public string NameSingular { get; set; }
        public string NamePlural { get; set; }
        public string Description { get; set; }
        public Guid? Icon { get; set; }
        public string TableName { get; set; }
        public bool IsTableGenerated { get; set; }
        public string IsTableGeneratedDesc { get; set; }
        public bool HasChatRoom { get; set; }
        public string HasChatRoomDesc { get; set; }
        public bool IsActivity { get; set; }
        public string IsActivityDesc { get; set; }
        public bool HasSecurity { get; set; }
        public string HasSecurityDesc { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }

        public byte[] IconBytes { get; set; }
    }
}
