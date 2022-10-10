namespace AlgoriaCore.Application.Managers.Questionnaires.Dto
{
    public class QuestionnaireCustomPropertiesDto
    {
        public string Currency { get; set; }
        public string Locale { get; set; }
        public long? MaxValue { get; set; }
        public long? MinValue { get; set; }
        public bool UseGrouping { get; set; }
    }
}
