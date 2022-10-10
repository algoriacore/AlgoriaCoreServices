using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Tenant
{
    public class TenantSettingsSendTestEmailCommandHandler : BaseCoreClass, IRequestHandler<TenantSettingsSendTestEmailCommand, int>
    {
        private readonly SettingManager _managerSetting;

        public TenantSettingsSendTestEmailCommandHandler(ICoreServices coreServices
        , SettingManager managerSetting) : base(coreServices)
        {
            _managerSetting = managerSetting;
        }

        public async Task<int> Handle(TenantSettingsSendTestEmailCommand request, CancellationToken cancellationToken)
        {
            await _managerSetting.SendTestEmail(new SendTestEmailDto()
            {
                EmailAddress = request.EmailAddress
            });

            return 0;
        }
    }
}
