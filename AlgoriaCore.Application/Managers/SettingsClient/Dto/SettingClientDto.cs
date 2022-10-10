namespace AlgoriaCore.Application.Managers.SettingsClient.Dto
{
    public class SettingClientDto
    {
        public long? Id { get; set; }
        public long? User { get; set; }
        public string ClientType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
