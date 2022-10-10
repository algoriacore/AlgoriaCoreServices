using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Utils;
using Autofac;

namespace AlgoriaCore.Application.Localization
{
    public class AppLocalizationProvider : IAppLocalizationProvider
    {
        private LanguageManager _managerLanguage;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ICacheLanguageService _cacheLanguageService;
        private readonly ICacheLanguageXmlService _cacheLanguageXMLService;
        private LanguageDto _languageDefault;

        public AppLocalizationProvider(ILifetimeScope lifetimeScope, ICacheLanguageService cacheLanguageService
        , ICacheLanguageXmlService cacheLanguageXMLService
        )
        {
            _lifetimeScope = lifetimeScope;
            _cacheLanguageService = cacheLanguageService;
            _cacheLanguageXMLService = cacheLanguageXMLService;
        }

        public string L(string key)
        {
            LanguageDto languageDefaultDto = GetLanguageDefault();
            int? languageId = languageDefaultDto == null ? null : languageDefaultDto.Id;
            int? tenantId = GetLanguageManager().CurrentUnitOfWork.GetTenantId();
            string label = null;

            if (languageId.HasValue)
            {
                label = _cacheLanguageService.GetEntry(tenantId, languageId, key);

                if (label.IsNullOrWhiteSpace())
                {
                    LanguageTextDto dtoText;
                    var manager = GetLanguageManager();

                    if (languageDefaultDto != null && languageDefaultDto.TenantId.HasValue)
                    {
                        dtoText = AsyncUtil.RunSync(() => manager.GetLanguageTextByLanguageAndKeyOrFromXMLAsync(languageId.Value, key));
                    } else 
                    {
                        dtoText = AsyncUtil.RunSync(() => manager.GetLanguageTextByLanguageAndKeyOrFromXMLByHostAsync(languageId.Value, key));
                    }

                    if (languageDefaultDto != null && (dtoText == null || dtoText.Value.IsNullOrWhiteSpace()))
                    {
                        label = _cacheLanguageXMLService.GetEntry(languageDefaultDto.Name, key);
                    } else 
                    {
                        label = dtoText.Value;
                    }
                }
            } 

            if (label.IsNullOrWhiteSpace())
            {
                label = _cacheLanguageXMLService.GetEntry(key);
            }

            _cacheLanguageService.SetEntry(tenantId, languageId, key, label);

            return label;
        }

        public string L(string key, params string[] parameters)
        {
            var t = L(key);
            var i = 0;
            foreach (var p in parameters)
            {
                t = t.Replace("{" + i + "}", p);
                i++;
            }

            return t;
        }

        private LanguageManager GetLanguageManager()
        {
            if (_managerLanguage == null)
            {
                _managerLanguage = _lifetimeScope.Resolve<LanguageManager>();
            }

            return _managerLanguage;
        }

        private LanguageDto GetLanguageDefault()
        {
            if (_languageDefault == null)
            {
                var manager = GetLanguageManager();
                _languageDefault = AsyncUtil.RunSync(() => manager.GetLanguageDefaultAsync());
            }

            return _languageDefault;
        }
    }
}
