using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.SettingsClient;
using AlgoriaCore.Application.Managers.SettingsClient.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SettingsClient
{
    public class SettingClientChangeCommandHandler : BaseCoreClass, IRequestHandler<SettingClientChangeCommand, long>
    {
        private readonly SettingClientManager _managerSettingClient;

        public SettingClientChangeCommandHandler(ICoreServices coreServices, 
            SettingClientManager managerSettingClient
        ) : base(coreServices)
        {
            _managerSettingClient = managerSettingClient;
        }

        public async Task<long> Handle(SettingClientChangeCommand request, CancellationToken cancellationToken)
        {
            SettingClientDto dto = new SettingClientDto()
            {
                ClientType = request.ClientType,
                User = SessionContext.UserId.Value,
                Name = request.Name,
                Value = request.Value
            };

            return await _managerSettingClient.ChangeSettingClient(dto);
        }
    }
}
