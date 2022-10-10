using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SettingsClient
{
    public class SettingClientChangeCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string ClientType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}