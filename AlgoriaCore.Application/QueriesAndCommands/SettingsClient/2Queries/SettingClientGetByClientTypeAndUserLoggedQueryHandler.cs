using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.SettingsClient;
using AlgoriaCore.Application.Managers.SettingsClient.Dto;
using AlgoriaCore.Application.QueriesAndCommands.SettingsClient;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SettingClient
{
    public class SettingClientGetByClientTypeAndUserLoggedQueryHandler : BaseCoreClass, IRequestHandler<SettingClientGetByClientTypeAndUserLoggedQuery, List<SettingClientResponse>>
    {
        private readonly SettingClientManager _managerSettingClient;

        public SettingClientGetByClientTypeAndUserLoggedQueryHandler(ICoreServices coreServices
        , SettingClientManager managerSettingClient)
                                : base(coreServices)
        {
            _managerSettingClient = managerSettingClient;
        }

        public async Task<List<SettingClientResponse>> Handle(SettingClientGetByClientTypeAndUserLoggedQuery request, CancellationToken cancellationToken)
        {
            List<SettingClientDto> listDto = await _managerSettingClient.GetSettingClientByClientTypeAndUserLogged(request.ClientType);
            List<SettingClientResponse> ll = new List<SettingClientResponse>();

            foreach (SettingClientDto dto in listDto)
            {
                ll.Add(new SettingClientResponse()
                {
                    Id = dto.Id.Value,
                    ClientType = dto.ClientType,
                    User = dto.User,
                    Name = dto.Name,
                    Value = dto.Value
                });
            }

            return ll;
        }
    }
}
