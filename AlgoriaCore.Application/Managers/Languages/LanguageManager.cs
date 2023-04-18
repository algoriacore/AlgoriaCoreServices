using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.Languages.Dto;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Languages
{
    public class LanguageManager : BaseManager
    {
        private readonly IRepository<Language, int> _repository;
        private readonly IRepository<LanguageText, long> _repositoryLanguageText;

        private readonly SettingManager _managerSetting;
        private readonly ICacheLanguageXmlService _cacheLanguageXMLService;
        private readonly ICacheLanguageService _cacheLanguageService;

        public LanguageManager(IRepository<Language, int> repository
        , IRepository<LanguageText, long> repositoryLanguageText
        , SettingManager managerSetting
        , ICacheLanguageXmlService cacheLanguageXMLService
        , ICacheLanguageService cacheLanguageService)
        {
            _repository = repository;
            _repositoryLanguageText = repositoryLanguageText;

            _managerSetting = managerSetting;
            _cacheLanguageXMLService = cacheLanguageXMLService;
            _cacheLanguageService = cacheLanguageService;
        }

        #region Lenguajes

        public async Task<PagedResultDto<LanguageDto>> GetLanguageListAsync(LanguageListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();
            string yesLabel = L("Yes");
            string noLabel = L("No");

            var query = (from entity in _repository.GetAll()
                         select new LanguageDto
                         {
                             TenantId = entity.TenantId,
                             Id = entity.Id,
                             Name = entity.Name,
                             DisplayName = entity.DisplayName,
                             IsActive = entity.IsActive == true,
                             IsActiveDesc = entity.IsActive == true ? yesLabel : noLabel
                         })
                         .WhereIf(
                            filter != null,
                            p => p.Id.ToString().Contains(filter)
                            || p.Name.ToUpper().Contains(filter)
                            || p.DisplayName.ToUpper().Contains(filter)
                            || p.IsActiveDesc.ToUpper().Contains(filter)
                         );

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "DisplayName" : dto.Sorting)
                .PageByIf(dto.IsPaged, dto)
                .ToListAsync();

            return new PagedResultDto<LanguageDto>(count, ll);
        }

        public async Task<List<LanguageDto>> GetLanguageActiveAsync()
        {
            var query = GetLanguageQuery();

            return await query.Where(p => p.IsActive).ToListAsync();
        }

        public async Task<LanguageDto> GetLanguageAsync(int id, bool throwExceptionIfNotFound = true)
        {
            var query = GetLanguageQuery();

            LanguageDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Languages.Language"), id));
            }

            return dto;
        }

        public async Task<LanguageDto> GetLanguageByNameAsync(string name)
        {
            var query = GetLanguageQuery();

            return await query.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<List<LanguageDto>> GetLanguageByNameListAsync(string name)
        {
            var query = GetLanguageQuery();

            return await query.Where(p => p.Name == name).ToListAsync();
        }

        public async Task<LanguageDto> GetLanguageFirstOrDefaultAsync()
        {
            var query = GetLanguageQuery();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<LanguageDto> GetLanguageDefaultAsync(bool onlyTenantOrHost = false)
        {
            LanguageDto dto = null;
            string stt = null;

            if (onlyTenantOrHost)
            {
                stt = await _managerSetting.GetSettingValueAsync(AppSettings.General.LanguageDefault);
            }
            else
            {
                stt = await _managerSetting.GetSettingValueAsync(AppSettings.General.LanguageDefault, SessionContext.UserId);

                if (stt.IsNullOrEmpty())
                {
                    stt = await _managerSetting.GetSettingValueAsync(AppSettings.General.LanguageDefault);
                }
            }

            if (!stt.IsNullOrEmpty())
            {
                dto = await GetLanguageAsync(int.Parse(stt), false);
            }

            if (dto == null)
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    dto = await GetLanguageFirstOrDefaultAsync();
                }
            }

            return dto;
        }

        public async Task<int> CreateLanguageAsync(LanguageDto dto)
        {
            await ValidateLanguageAsync(dto);

            var entity = new Language();

            entity.TenantId = CurrentUnitOfWork.GetTenantId();
            entity.Name = dto.Name;
            entity.DisplayName = dto.DisplayName;
            entity.IsActive = dto.IsActive;

            entity.Id = _repository.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeLanguage(await GetLanguageAsync(entity.Id), null, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task UpdateLanguageAsync(LanguageDto dto)
        {
            await ValidateLanguageAsync(dto);

            var entity = await _repository.FirstOrDefaultAsync(dto.Id.Value);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Languages"), dto.Id));
            }

            LanguageDto previousDto = await GetLanguageAsync(dto.Id.Value);

            entity.Name = dto.Name;
            entity.DisplayName = dto.DisplayName;
            entity.IsActive = dto.IsActive;

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeLanguage(await GetLanguageAsync(entity.Id), previousDto, ChangeLogType.Update);
        }

        public async Task DeleteLanguageAsync(int id)
        {
            LanguageDto previousDto = await GetLanguageAsync(id);

            if (previousDto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Languages"), id));
            }

            await DeleteLanguageTextByLanguageAsync(id);
            await _repository.DeleteAsync(id);

            await LogChangeLanguage(null, previousDto, ChangeLogType.Delete);
        }

        public void SetLanguageDefault(int language)
        {
            _managerSetting.ChangeSetting(AppSettings.General.LanguageDefault, language.ToString());
        }

        public async Task<List<ComboboxItemDto>> GetLanguageCombo()
        {
            var query = (from entity in _repository.GetAll()
                         where entity.IsActive == true
                         orderby entity.DisplayName
                         select new LanguageDto
                         {
                             Id = entity.Id,
                             DisplayName = entity.DisplayName
                         });

            List<LanguageDto> list = await query.ToListAsync();

            return list.Select(p => new ComboboxItemDto(p.Id.ToString(), p.DisplayName)).ToList();
        }

        #region Private Methods

        private IQueryable<LanguageDto> GetLanguageQuery()
        {
            var query = (from entity in _repository.GetAll()
                         select new LanguageDto
                         {
                             TenantId = entity.TenantId,
                             Id = entity.Id,
                             Name = entity.Name,
                             DisplayName = entity.DisplayName,
                             IsActive = entity.IsActive == true,
                         });

            return query;
        }

        private async Task ValidateLanguageAsync(LanguageDto dto)
        {
            List<LanguageDto> list = await GetLanguageByNameListAsync(dto.Name);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("Languages.Language.DuplicatedName"));
            }
        }

        private async Task<long> LogChangeLanguage(LanguageDto newDto, LanguageDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new LanguageDto(); }
            if (previousDto == null) { previousDto = new LanguageDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
				LogStringProperty(sb, previousDto.Name, newDto.Name, "{{Languages.Language.Name}}");
				LogStringProperty(sb, previousDto.DisplayName, newDto.DisplayName, "{{Languages.Language.DisplayName}}");

                if (newDto.IsActive != previousDto.IsActive)
                {
                    sb.AppendFormat("{0}: {1} => {2}\n", "{{IsActive}}", previousDto.IsActive ? "{{Yes}}" : "{{No}}", newDto.IsActive ? "{{Yes}}" : "{{No}}");
                }
            }

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "Language", sb.ToString());
        }

        #endregion

        #endregion

        #region Textos

        public async Task<PagedResultDto<LanguageTextDto>> GetLanguageTextListAsync(LanguageTextListFilterDto dto)
        {
            LanguageDto languageDto = await GetLanguageAsync(dto.LanguageId.Value);
            var all = await GetLanguageTextMergedWithXMLAsync(languageDto);
            var query = all.AsQueryable();

            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();
            query = query.WhereIf(
                    filter != null,
                    p => p.Key.ToUpper().Contains(filter)
                    || p.Value.ToUpper().Contains(filter)
                );

            var count = query.Count();
            var ll = query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "Key" : dto.Sorting)
                .PageBy(dto)
                .ToList();

            return new PagedResultDto<LanguageTextDto>(count, ll);
        }

        public async Task<List<LanguageTextDto>> GetLanguageTextDefaultAsync()
        {
			List<LanguageTextDto> list;
            LanguageDto languageDefaultDto = await GetLanguageDefaultAsync();

            if (languageDefaultDto == null)
            {
                list = GetLanguageTextFromXML();
            }
            else
            {
                using (CurrentUnitOfWork.SetTenantId(languageDefaultDto.TenantId))
                {
                    list = await GetLanguageTextMergedWithXMLAsync(languageDefaultDto);
                }
            }

            return list;
        }

        public async Task<LanguageTextDto> GetLanguageTextByLanguageAndKeyOrFromXMLAsync(int language, string key)
        {
            List<LanguageTextDto> list = await GetLanguageTextByLanguageAndKeyListAsync(language, key);
            LanguageTextDto dto = list.FirstOrDefault();

            if (dto == null)
            {
                list = await GetLanguageTextFromXML(language);

                dto = list.FirstOrDefault(p => p.Key == key);
            }

            return dto;
        }

        public async Task<LanguageTextDto> GetLanguageTextByLanguageAndKeyOrFromXMLByHostAsync(int language, string key)
        {
            LanguageTextDto dto = null;

            using (CurrentUnitOfWork.SetTenantId(null))
            {
                dto = await GetLanguageTextByLanguageAndKeyOrFromXMLAsync(language, key);
            }

            return dto;
        }

        public async Task<LanguageTextDto> GetLanguageTextAsync(long id)
        {
            var query = GetLanguageTextQuery();

            LanguageTextDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Languages.Texts.Text"), id));
            }

            return dto;
        }

        public async Task<List<LanguageTextDto>> GetLanguageTextByLanguageAndKeyListAsync(int language, string key)
        {
            var query = GetLanguageTextQuery();

            return await query.Where(p => p.LanguageId == language && p.Key.ToUpper() == key.ToUpper()).ToListAsync();
        }

        public async Task<LanguageTextDto> GetLanguageTextByLanguageCodeAndKeyAsync(string languageCode, string key)
        {
            var query = GetLanguageTextQuery();

            return await query.FirstOrDefaultAsync(p => p.LanguageName == languageCode && p.Key.ToUpper() == key.ToUpper());
        }

        public async Task<List<LanguageTextDto>> GetLanguageTextFromXML(int language)
        {
            LanguageDto dto = await GetLanguageAsync(language, false);

            return GetLanguageTextFromXML(dto != null ? dto.Name : string.Empty);
        }

        public List<LanguageTextDto> GetLanguageTextFromXML(string languageName = "", bool takeDefaultIsNotExists = true)
        {
            return _cacheLanguageXMLService.GetLanguageTextFromXML(languageName, takeDefaultIsNotExists);
        }

        public async Task<long> CreateLanguageTextAsync(LanguageTextDto dto)
        {
            await ValidateLanguageTextAsync(dto);

            var entity = new LanguageText();

            entity.LanguageId = dto.LanguageId;
            entity.Key = dto.Key;
            entity.Value = dto.Value;

            entity.Id = _repositoryLanguageText.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            _cacheLanguageService.RemoveEntry(CurrentUnitOfWork.GetTenantId(), entity.LanguageId, entity.Key);

            return entity.Id;
        }

        public async Task<long> UpdateLanguageTextAsync(LanguageTextDto dto)
        {
            List<LanguageTextDto> list = await GetLanguageTextByLanguageAndKeyListAsync(dto.LanguageId.Value, dto.Key);
            LanguageTextDto dtoDB = list.FirstOrDefault();
            LanguageText entity;
            ChangeLogType changeLogType = ChangeLogType.Update;

            if (dtoDB == null)
            {
                entity = new LanguageText();
                entity.LanguageId = dto.LanguageId;
                entity.Key = dto.Key;
                entity.Value = dto.Value;

                entity.Id = _repositoryLanguageText.InsertAndGetId(entity);

                changeLogType = ChangeLogType.Create;
            }
            else
            {
                entity = await _repositoryLanguageText.FirstOrDefaultAsync(dtoDB.Id.Value);
                entity.Value = dto.Value;
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeLanguageText(await GetLanguageTextAsync(entity.Id), dtoDB, changeLogType);

            _cacheLanguageService.RemoveEntry(CurrentUnitOfWork.GetTenantId(), entity.LanguageId, entity.Key);

            return entity.Id;
        }

        public async Task DeleteLanguageTextByLanguageAsync(int language)
        {
            await _repositoryLanguageText.DeleteAsync(p => p.LanguageId == language);

            _cacheLanguageService.CancelEntryParentLanguage(CurrentUnitOfWork.GetTenantId(), language);
        }

        #region Private Methods

        private IQueryable<LanguageTextDto> GetLanguageTextQuery()
        {
            var query = (from entity in _repositoryLanguageText.GetAll()
                         select new LanguageTextDto
                         {
                             Id = entity.Id,
                             LanguageId = entity.LanguageId,
                             LanguageName = entity.Language.Name,
                             Key = entity.Key,
                             Value = entity.Value
                         });

            return query;
        }

        private async Task ValidateLanguageTextAsync(LanguageTextDto dto)
        {
            List<LanguageTextDto> list = await GetLanguageTextByLanguageAndKeyListAsync(dto.LanguageId.Value, dto.Key);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("Languages.Texts.Text.DuplicatedKey"));
            }
        }

        private async Task<List<LanguageTextDto>> GetLanguageTextByLanguageAsync(int languageId)
        {
            var query = GetLanguageTextQuery();

            return await query.Where(p => p.LanguageId == languageId).ToListAsync();

        }

        private async Task<List<LanguageTextDto>> GetLanguageTextMergedWithXMLAsync(LanguageDto languageDto)
        {
            List<LanguageTextDto> listDB = await GetLanguageTextByLanguageAsync(languageDto.Id.Value);
            return GetLanguageTextMergedWithXMLAux(languageDto, listDB);
        }

        private List<LanguageTextDto> GetLanguageTextMergedWithXMLAux(LanguageDto languageDto, List<LanguageTextDto> listDB)
        {
            List<LanguageTextDto> listXML = GetLanguageTextFromXML(languageDto.Name);
            LanguageTextDto dtoDB = null;

            foreach (LanguageTextDto dtoXML in listXML)
            {
                dtoDB = listDB.FirstOrDefault(p => p.Key == dtoXML.Key);

                if (dtoDB != null)
                {
                    dtoXML.Id = dtoDB.Id;
                    dtoXML.Value = dtoDB.Value;
                }

                dtoXML.LanguageId = languageDto.Id;
            }

            return listXML;
        }

        private async Task<long> LogChangeLanguageText(LanguageTextDto newDto, LanguageTextDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new LanguageTextDto(); }
            if (previousDto == null) { previousDto = new LanguageTextDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
                if (newDto.Key != previousDto.Key)
                {
                    sb.AppendFormat("{0}: {1} => {2}\n", "{{Languages.Texts.Text.Key}}", previousDto.Key ?? string.Empty, newDto.Key ?? string.Empty);
                }

                if (newDto.Value != previousDto.Value)
                {
                    sb.AppendFormat("{0}: {1} => {2}\n", "{{Languages.Texts.Text.Value}}", previousDto.Value ?? string.Empty, newDto.Value ?? string.Empty);
                }
            }

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "LanguageText", sb.ToString());
        }

        #endregion

        #endregion
    }
}
