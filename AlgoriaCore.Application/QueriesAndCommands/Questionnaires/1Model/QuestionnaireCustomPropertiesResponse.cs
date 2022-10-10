namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireCustomPropertiesResponse
    {
        public string Currency { get; set; }
        public string Locale { get; set; }
        public long? MaxValue { get; set; }
        public long? MinValue { get; set; }
        public bool UseGrouping { get; set; }
    }
}