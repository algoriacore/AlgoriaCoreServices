namespace AlgoriaCore.Application.Managers.Templates.Dto
{
    public class TemplateQueryDto
    {
        public long? Id { get; set; }
        public long Template { get; set; }
        public TemplateQueryType QueryType { get; set; }
        public string Query { get; set; }
    }

    public enum TemplateQueryType : byte 
    {
        View = 1,
        ViewFilters = 2,
        Insert = 3,
        Read = 4,
        Update = 5,
        Delete = 6
    }
}
