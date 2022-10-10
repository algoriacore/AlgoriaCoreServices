using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.SettingsClient
{
    public class SettingClientGetByClientTypeAndUserLoggedQuery : PageListByDto, IRequest<List<SettingClientResponse>>
    {
        public string ClientType { get; set; }
    }
}
