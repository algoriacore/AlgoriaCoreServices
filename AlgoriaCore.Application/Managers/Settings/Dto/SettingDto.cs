namespace AlgoriaCore.Application.Managers.Settings.Dto
{
    public class SettingDto
    {
        public long? Id { get; set; }
        public int? TenantId { get; set; }
        public long? UserId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
