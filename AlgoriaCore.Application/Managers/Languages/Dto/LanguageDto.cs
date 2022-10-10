namespace AlgoriaCore.Application.Managers.Languages.Dto
{
    public class LanguageDto
    {
        public int? TenantId { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
    }
}
