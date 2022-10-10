namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionForListResponse
    {
        public long Id { get; set; }
        public long? Template { get; set; }
        public string TemplateDesc { get; set; }
        public string Name { get; set; }
        public short? Order { get; set; }
        public string IconAF { get; set; }
    }
}