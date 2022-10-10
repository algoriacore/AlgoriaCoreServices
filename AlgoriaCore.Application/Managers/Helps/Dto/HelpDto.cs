namespace AlgoriaCore.Application.Managers.Helps.Dto
{
    public class HelpDto
    {
        public long? Id { get; set; }
        public int Language { get; set; }
        public string LanguageDesc { get; set; }
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
    }
}
