using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Application.Managers.SettingsClient;
using AlgoriaCore.Application.Managers.SettingsClient.Dto;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.QueriesAndCommands.Users._1Model;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Utils;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserProfileQueryHandler : BaseCoreClass, IRequestHandler<UserProfileQuery, UserForEditResponse>
    {
        private readonly UserManager _userManager;
        private readonly SettingClientManager _managerSettingClient;
        private readonly LanguageManager _managerLanguage;
        private readonly SettingManager _managerSetting;

        public UserProfileQueryHandler(ICoreServices coreServices,
            UserManager userManager,
            SettingClientManager managerSettingClient,
            LanguageManager managerLanguage,
            SettingManager managerSetting
        ) : base(coreServices)
        {
            _userManager = userManager;
            _managerSettingClient = managerSettingClient;
            _managerLanguage = managerLanguage;
            _managerSetting = managerSetting;
        }

        public async Task<UserForEditResponse> Handle(UserProfileQuery request, CancellationToken cancellationToken)
        {
            var dto = await _userManager.GetUserOfSessionAsync();

            List<SettingClientDto> settingsClientList = await _managerSettingClient.GetSettingClientByClientTypeAndUser(request.ClientType, dto.Id);
            List<ComboboxItemDto> languageComboDto = await _managerLanguage.GetLanguageCombo();

            var languageSetting = await _managerSetting.GetSettingValueAsync(AppSettings.General.LanguageDefault, SessionContext.UserId);

            if (languageSetting.IsNullOrEmpty())
            {
                languageSetting = await _managerSetting.GetSettingValueAsync(AppSettings.General.LanguageDefault);
            }

            if (languageSetting.IsNullOrEmpty())
            {
                languageSetting = await _managerSetting.GetSettingValueByHostAsync(AppSettings.General.LanguageDefault);
            }

            LanguageDto languageDto = null;

            if (languageSetting != null)
            {
                languageDto = GetLanguageDto(languageSetting, ref languageComboDto);
            }

            var resp = new UserForEditResponse
            {
                Id = dto.Id,
                UserName = dto.Login,
                Name = dto.Name,
                LastName = dto.LastName,
                SecondLastName = dto.SecondLastName,
                EmailAddress = dto.EmailAddress,
                PhoneNumber = dto.PhoneNumber,
                IsActive = dto.IsActive,
                Preferences = settingsClientList.ToDictionary(p => p.Name, p => p.Value),
                Language = languageDto == null ? int.Parse(languageSetting) : languageDto.Id,
                LanguageCombo = languageComboDto.Select(p => new ComboboxItemDto(p.Value, p.Label)).ToList()
            };

            return resp;
        }

        private LanguageDto GetLanguageDto(string languageSetting, ref List<ComboboxItemDto> languageComboDto)
		{
			LanguageDto languageDto = null;

			if (languageSetting.IsNullOrEmpty())
			{
				languageDto = AsyncUtil.RunSync(() => _managerLanguage.GetLanguageDefaultAsync());
			}
			else
			{
				if (!languageComboDto.Exists(p => p.Value == languageSetting))
				{
					int languageId = int.Parse(languageSetting);

					languageDto = AsyncUtil.RunSync(() => _managerLanguage.GetLanguageAsync(languageId, false));

					if (languageDto == null && _userManager.CurrentUnitOfWork.GetTenantId() != null)
					{
						using (_userManager.CurrentUnitOfWork.SetTenantId(null))
						{
							languageDto = AsyncUtil.RunSync(() => _managerLanguage.GetLanguageAsync(languageId, false));
						}
					}
				}
			}

			if (languageDto != null)
			{
				languageComboDto.Add(new ComboboxItemDto(languageDto.Id.ToString(), languageDto.DisplayName));
				languageComboDto = languageComboDto.OrderBy(p => p.Label).ToList();
			}

			return languageDto;
		}
    }
}
