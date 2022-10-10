using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Tenant
{
    public class TenantSettingsSendTestEmailCommand : IRequest<int>
    {
        public string EmailAddress { get; set; }
    }
}