namespace AlgoriaCore.Application.Managers.Templates.Dto
{
    public class TemplateSecurityMemberDto
    {
        public long? Id { get; set; }
        public long Template { get; set; }
        public SecurityMemberType Type { get; set; }
        public string TypeDesc { get; set; }
        public long Member { get; set; }
        public string MemberDesc { get; set; }
        public SecurityMemberLevel Level { get; set; }
        public string LevelDesc { get; set; }
        public bool IsExecutor { get; set; }
        public string IsExecutorDesc { get; set; }

        public long Total { get; set; }
    }

    public enum SecurityMemberType: byte 
    {
        User = 1,
        OrgUnit = 2
    }

    public enum SecurityMemberLevel : byte
    {
        Reader = 1,
        Editor = 2
    }
}
