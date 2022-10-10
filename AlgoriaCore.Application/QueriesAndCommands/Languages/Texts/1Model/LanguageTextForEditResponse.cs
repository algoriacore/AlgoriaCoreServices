namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextForEditResponse
    {
        public long? Id { get; set; }
        public int? LanguageId { get; set; }
        public string LanguageDesc { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}