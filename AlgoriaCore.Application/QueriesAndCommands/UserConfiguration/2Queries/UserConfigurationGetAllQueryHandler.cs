using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.UserConfiguration;
using AlgoriaCore.Application.Managers.UserConfiguration.Dto;
using AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom;
using AlgoriaCore.Application.QueriesAndCommands.MultiTenancy;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.UserConfiguration
{
    public class UserConfigurationGetAllQueryHandler : BaseCoreClass, IRequestHandler<UserConfigurationGetAllQuery, UserConfigurationResponse>
    {
        private readonly UserConfigurationManager _manager;

        public UserConfigurationGetAllQueryHandler(ICoreServices coreServices
        , UserConfigurationManager manager)
                                : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<UserConfigurationResponse> Handle(UserConfigurationGetAllQuery request, CancellationToken cancellationToken)
        {
            UserConfigurationDto dto = await _manager.GetAllAsync(new UserConfigurationFilterDto() 
            {
                ClientType = request.ClientType 
            });

            return new UserConfigurationResponse()
            {
                Localization = new UserLocalizationConfigResponse()
                {
                    DefaultLanguage = new LanguageInfoResponse()
                    {
                        Name = dto.Localization.DefaultLanguage.Name,
                        DisplayName = dto.Localization.DefaultLanguage.DisplayName
                    },
                    CurrentLanguage = new LanguageInfoResponse()
                    {
                        Name = dto.Localization.CurrentLanguage.Name,
                        DisplayName = dto.Localization.CurrentLanguage.DisplayName
                    },
                    Values = dto.Localization.Values
                },
                Permission = new _1Model.UserPermissionConfigResponse() {
                    Values = dto.Permission.Values
                },
                SettingsClient = dto.SettingsClient,
                CatalogsCustom = dto.CatalogsCustom.Select(p => new CatalogCustomResponse()
                {
                    Id = p.Id,
                    NameSingular = p.NameSingular,
                    NamePlural = p.NamePlural,
                    Description = p.Description,
                    Questionnaire = p.Questionnaire
                }).ToList(),
                MultiTenancyConfig = new MultiTenancyConfigResponse()
                {
                    Enabled = dto.MultiTenancyConfig.Enabled,
                    TenancyNameDefault = dto.MultiTenancyConfig.TenancyNameDefault
                },
                PasswordComplexity = dto.PasswordComplexity
            };
        }
    }
}
