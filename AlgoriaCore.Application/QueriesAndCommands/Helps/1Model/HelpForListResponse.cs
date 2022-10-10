namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpForListResponse
    {
        public long Id { get; set; }
        public string LanguageDesc { get; set; }
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
    }
}