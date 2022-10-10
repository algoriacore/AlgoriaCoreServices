namespace AlgoriaCore.Application.Managers.Settings.Dto
{
    public class PasswordComplexityDto
    {
        public byte MinimumLength { get; set; }
        public byte MaximumLength { get; set; }
        public bool UseNumbers { get; set; }
        public bool UseUppercase { get; set; }
        public bool UseLowercase { get; set; }
        public bool UsePunctuationSymbols { get; set; }
    }
}
