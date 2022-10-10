using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Host
{
    public class HostSettingsSendTestEmailCommandHandler : BaseCoreClass, IRequestHandler<HostSettingsSendTestEmailCommand, int>
    {
        private readonly SettingManager _managerSetting;

        public HostSettingsSendTestEmailCommandHandler(ICoreServices coreServices
        , SettingManager managerSetting) : base(coreServices)
        {
            _managerSetting = managerSetting;
        }

        public async Task<int> Handle(HostSettingsSendTestEmailCommand request, CancellationToken cancellationToken)
        {
            await _managerSetting.SendTestEmail(new SendTestEmailDto()
            {
                EmailAddress = request.EmailAddress
            });

            return 0;
        }
    }
}
