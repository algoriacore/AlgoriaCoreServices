﻿using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using AlgoriaCore.Application.Managers.Permissions;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Application.Managers.SettingsClient;
using AlgoriaCore.Application.Managers.SettingsClient.Dto;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Application.Managers.UserConfiguration.Dto;
using AlgoriaCore.Application.MultiTenancy;
using AlgoriaCore.Domain.Interfaces.MultiTenancy;
using AlgoriaPersistence.Interfaces.Interfaces;
using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.UserConfiguration
{
    public class UserConfigurationManager : BaseManager
    {
        private readonly LanguageManager _managerLanguage;
        private readonly PermissionManager _permissionManager;
        private readonly SettingClientManager _settingClientManager;
        private readonly TenantManager _managerTenant;
        private readonly SettingManager _managerSetting;
        
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IMongoDBContext _context;
		private readonly ILifetimeScope _lifetimeScope;

        public UserConfigurationManager(
            LanguageManager managerLanguage,
            PermissionManager permissionManager,
            SettingClientManager settingClientManager,
            TenantManager managerTenant,
            SettingManager managerSetting,
            IMultiTenancyConfig multiTenancyConfig,
            IMongoDBContext context,
            ILifetimeScope lifetimeScope
        )
        {
            _managerLanguage = managerLanguage;
            _permissionManager = permissionManager;
            _settingClientManager = settingClientManager;
            _managerTenant = managerTenant;
            _managerSetting = managerSetting;
            _multiTenancyConfig = multiTenancyConfig;
            _context = context;
            _lifetimeScope = lifetimeScope;
        }

        public async Task<UserConfigurationDto> GetAllAsync(UserConfigurationFilterDto filterDto)
        {
            UserConfigurationDto dto = new UserConfigurationDto();
            int? tenantId;

            dto.MultiTenancyConfig = new MultiTenancyConfigDto() { 
                Enabled = _multiTenancyConfig.IsEnabled(),
                TenancyNameDefault = _multiTenancyConfig.GetTenancyNameDefault()
            };

            if (dto.MultiTenancyConfig.Enabled) {
                tenantId = CurrentUnitOfWork.GetTenantId();
            } else {
                tenantId = (await _managerTenant.GetTenantByTenancyNameAsync(dto.MultiTenancyConfig.TenancyNameDefault)).Id;
            }

            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                LanguageDto defaultLanguage = await _managerLanguage.GetLanguageDefaultAsync();
                LanguageDto defaultLanguageTenantOrHost = await _managerLanguage.GetLanguageDefaultAsync(true);
                List<LanguageTextDto> listLanguageTextsDefault = await _managerLanguage.GetLanguageTextDefaultAsync();

                dto.Localization = new UserLocalizationConfigDto()
                {
                    DefaultLanguage = defaultLanguageTenantOrHost,
                    CurrentLanguage = defaultLanguage,
                    Values = listLanguageTextsDefault.ToDictionary(p => p.Key, p => p.Value)
                };

                var permission = await _permissionManager.GetPermissionListByUserAsync();

                dto.Permission = new UserPermissionConfigDto
                {
                    Values = permission.ToDictionary((string permissionName) => permissionName, (string permissionName) => true)
                };

                if (SessionContext.UserId.HasValue)
                {
                    List<SettingClientDto> settingsClientList = await _settingClientManager.GetSettingClientByClientTypeAndUserLogged(filterDto.ClientType);
                    dto.SettingsClient = settingsClientList.ToDictionary(p => p.Name, p => p.Value);
                }

                if (tenantId.HasValue && _context.IsEnabled && _context.IsActive)
                {
                    var _managerCatalogCustom = _lifetimeScope.Resolve<CatalogCustomManager>();
                    dto.CatalogsCustom = _managerCatalogCustom.GetCatalogCustomActiveListAsync();
                }

                dto.PasswordComplexity = await _managerSetting.GetPasswordComplexityAsync();
            }

            return dto;
        }
    }
}
