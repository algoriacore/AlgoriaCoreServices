using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.Helps.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Helps
{
    public class HelpManager : BaseManager
    {
        private readonly IRepository<help, long> _repository;
        private readonly IRepository<helptxt, long> _repositoryHelpTxt;

        public HelpManager(IRepository<help, long> repository
        , IRepository<helptxt, long> repositoryHelpTxt)
        {
            _repository = repository;
            _repositoryHelpTxt = repositoryHelpTxt;
        }

        public async Task<PagedResultDto<HelpDto>> GetHelpListAsync(HelpListFilterDto dto)
        {
            var query = GetHelpListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty()? "DisplayName": dto.Sorting)
                .PageByIf(dto.IsPaged, dto)
                .ToListAsync();

            return new PagedResultDto<HelpDto>(count, ll);
        }

        public async Task<HelpDto> GetHelpAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetHelpQuery(true);

            HelpDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Helps.Help"), id));
            }

            return dto;

        }

        public async Task<HelpDto> GetHelpByLanguageAndKeyAsync(int language, string key)
        {
            var query = GetHelpQuery(true);

            return await query.FirstOrDefaultAsync(p => p.Language == language && p.Key.ToUpper() == key.ToUpper());
        }

        public async Task<List<HelpDto>> GetHelpByLanguageAndKeyListAsync(int language, string key)
        {
            var query = GetHelpQuery(true);

            return await query.Where(p => p.Language == language && p.Key.ToUpper() == key.ToUpper()).ToListAsync();
        }

        public async Task<long> CreateHelpAsync(HelpDto dto)
        {
            await ValidateHelpAsync(dto);

            var entity = new help();

            entity.LanguageId = dto.Language;
            entity.HelpKey = dto.Key;
            entity.DisplayName = dto.DisplayName;
            entity.IsActive = dto.IsActive;

            if (!dto.Body.IsNullOrWhiteSpace())
            {
                entity.helptxt = new helptxt() { body = dto.Body };
            }

            entity.Id = _repository.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeHelp(await GetHelpAsync(entity.Id), null, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task UpdateHelpAsync(HelpDto dto)
        {
            await ValidateHelpAsync(dto);

            var entity = await _repository.FirstOrDefaultAsync(dto.Id.Value);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Helps.Help"), dto.Id));
            }

            HelpDto previousDto = await GetHelpAsync(dto.Id.Value);

            entity.LanguageId = dto.Language;
            entity.HelpKey = dto.Key;
            entity.DisplayName = dto.DisplayName;
            entity.IsActive = dto.IsActive;

            if (dto.Body.IsNullOrWhiteSpace())
            {
                entity.helptxt = null;
            } else {
                entity.helptxt = await _repositoryHelpTxt.FirstOrDefaultAsync(p => p.help == entity.Id);

                if (entity.helptxt == null)
                {
                    entity.helptxt = new helptxt() { help = entity.Id };
                }

                entity.helptxt.body = dto.Body;
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeHelp(await GetHelpAsync(entity.Id), previousDto, ChangeLogType.Update);
        }

        public async Task DeleteHelpAsync(int id)
        {
            HelpDto previousDto = await GetHelpAsync(id);

            if (previousDto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Helps.Help"), id));
            }

            await _repositoryHelpTxt.DeleteAsync(p => p.help == id);
            await _repository.DeleteAsync(id);

            await LogChangeHelp(null, previousDto, ChangeLogType.Delete);
        }

        #region Private Methods

        private IQueryable<HelpDto> GetHelpQuery(bool isIncludeCuerpo = false)
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");

            var query = (from entity in _repository.GetAll()
                         select new HelpDto
                         {
                             Id = entity.Id,
                             Language = entity.LanguageId,
                             LanguageDesc = entity.Language.DisplayName,
                             Key = entity.HelpKey,
                             DisplayName = entity.DisplayName,
                             Body = isIncludeCuerpo ? entity.helptxt.body : null,
                             IsActive = entity.IsActive == true,
                             IsActiveDesc = entity.IsActive == true ? yesLabel : noLabel
                         });

            return query;
        }

        private IQueryable<HelpDto> GetHelpListQuery(HelpListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetHelpQuery(dto.IsIncludeBody)
                .WhereIf(
                    filter != null,
                    p => p.Id.ToString().Contains(filter)
                    || p.LanguageDesc.ToUpper().Contains(filter)
                    || p.Key.ToUpper().Contains(filter)
                    || p.DisplayName.ToUpper().Contains(filter)
                    || p.IsActiveDesc.ToUpper().Contains(filter)
                );

            return query;
        }

        private async Task ValidateHelpAsync(HelpDto dto)
        {
            List<HelpDto> list = await GetHelpByLanguageAndKeyListAsync(dto.Language, dto.Key);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("Helps.Help.DuplicatedKey"));
            }
        }

        private async Task<long> LogChangeHelp(HelpDto newDto, HelpDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new HelpDto(); }
            if (previousDto == null) { previousDto = new HelpDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
				LogStringProperty(sb, previousDto.LanguageDesc, newDto.LanguageDesc, "{{Helps.Help.Language}}");
				LogStringProperty(sb, previousDto.Key, newDto.Key, "{{Helps.Help.Key}}");
				LogStringProperty(sb, previousDto.DisplayName, newDto.DisplayName, "{{Helps.Help.DisplayName}}");

                if (newDto.IsActive != previousDto.IsActive)
                {
                    sb.AppendFormat("{0}: {1} => {2}\n", "{{IsActive}}", previousDto.IsActive ? "{{Yes}}" : "{{No}}", newDto.IsActive ? "{{Yes}}" : "{{No}}");
                }
            }

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "Help", sb.ToString());
        }

        #endregion
    }
}
