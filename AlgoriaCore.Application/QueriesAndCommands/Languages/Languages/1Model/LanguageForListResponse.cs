namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageForListResponse
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
    }
}