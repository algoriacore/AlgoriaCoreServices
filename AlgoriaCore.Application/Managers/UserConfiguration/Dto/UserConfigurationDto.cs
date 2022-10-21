using AlgoriaCore.Application.Managers.CatalogsCustom.Dto;
using AlgoriaCore.Application.Managers.Settings.Dto;
using AlgoriaCore.Application.MultiTenancy;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.UserConfiguration.Dto
{
    public class UserConfigurationDto
    {
        public UserLocalizationConfigDto Localization { get; set; }
        public UserPermissionConfigDto Permission { get; set; }
        public Dictionary<string, string> SettingsClient { get; set; }
        public List<CatalogCustomDto> CatalogsCustom { get; set; }
        public MultiTenancyConfigDto MultiTenancyConfig { get; set; }
        public PasswordComplexityDto PasswordComplexity { get; set; }

        public UserConfigurationDto()
        {
            SettingsClient = new Dictionary<string, string>();
            CatalogsCustom = new List<CatalogCustomDto>();
        }
    }
}
