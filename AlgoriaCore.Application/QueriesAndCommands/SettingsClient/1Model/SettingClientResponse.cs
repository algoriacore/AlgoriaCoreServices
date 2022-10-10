namespace AlgoriaCore.Application.QueriesAndCommands.SettingsClient
{
    public class SettingClientResponse
    {
        public long Id { get; set; }
        public long? User { get; set; }
        public string ClientType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}