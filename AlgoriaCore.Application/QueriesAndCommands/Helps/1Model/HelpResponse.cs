namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpResponse
    {
        public long? Id { get; set; }
        public int Language { get; set; }
        public string LanguageDesc { get; set; }
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
    }
}