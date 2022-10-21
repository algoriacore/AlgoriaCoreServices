using AlgoriaCore.Application.Managers.Settings.Dto;
using AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom;
using AlgoriaCore.Application.QueriesAndCommands.MultiTenancy;
using AlgoriaCore.Application.QueriesAndCommands.UserConfiguration._1Model;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.UserConfiguration
{
    public class UserConfigurationResponse
    {
        public UserLocalizationConfigResponse Localization { get; set; }
        public UserPermissionConfigResponse Permission { get; set; }
        public Dictionary<string, string> SettingsClient { get; set; }
        public List<CatalogCustomResponse> CatalogsCustom { get; set; }
        public MultiTenancyConfigResponse MultiTenancyConfig { get; set; }
        public PasswordComplexityDto PasswordComplexity { get; set; }

        public UserConfigurationResponse()
        {
            SettingsClient = new Dictionary<string, string>();
            CatalogsCustom = new List<CatalogCustomResponse>();
        }
    }
}