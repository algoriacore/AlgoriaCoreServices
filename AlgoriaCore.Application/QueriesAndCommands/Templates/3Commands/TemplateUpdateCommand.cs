using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateUpdateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string RGBColor { get; set; }
        public string NameSingular { get; set; }
        public string NamePlural { get; set; }
        public string Description { get; set; }
        public bool HasChatRoom { get; set; }
        public bool IsActivity { get; set; }
        public bool HasSecurity { get; set; }
        public bool IsActive { get; set; }
        public string IconName { get; set; }
    }
}