using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Host
{
    public class HostSettingsSendTestEmailCommand : IRequest<int>
    {
        public string EmailAddress { get; set; }
    }
}