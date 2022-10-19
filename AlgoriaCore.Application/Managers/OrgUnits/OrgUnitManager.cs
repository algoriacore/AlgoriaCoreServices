using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.OrgUnits.Dto;
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

namespace AlgoriaCore.Application.Managers.OrgUnits
{
    public class OrgUnitManager : BaseManager
    {
        private readonly IRepository<OrgUnit, long> _repository;
        private readonly IRepository<OrgUnitUser, long> _repositoryOrgUnitUser;

        public OrgUnitManager(
            IRepository<OrgUnit, long> repository,
            IRepository<OrgUnitUser, long> repositoryOrgUnitUser
        )
        {
            _repository = repository;
            _repositoryOrgUnitUser = repositoryOrgUnitUser;
        }

        #region Organization Units

        public async Task<PagedResultDto<OrgUnitDto>> GetOrgUnitListAsync(OrgUnitListFilterDto dto)
        {
            var query = GetOrgUnitListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty()? "Name": dto.Sorting)
                .PageBy(dto)
                .ToListAsync();

            return new PagedResultDto<OrgUnitDto>(count, ll);
        }

        public async Task<OrgUnitDto> GetOrgUnitAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetOrgUnitQuery();

            OrgUnitDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("OrgUnits.OrgUnit"), id));
            }

            return dto;

        }

        public async Task<List<OrgUnitDto>> GetOrgUnitByParentListAsync(long parent)
        {
            var query = GetOrgUnitQuery();

            return await query.Where(p => p.ParentOU == parent).OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<List<OrgUnitDto>> GetOrgUnitByNameListAsync(string name)
        {
            var query = GetOrgUnitQuery();

            return await query.Where(p => p.Name.ToUpper() == name.ToUpper()).ToListAsync();
        }

        public async Task<List<ComboboxItemDto>> GetOrgUnitComboAsync()
        {
            return await GetOrgUnitComboAsync(new OrgUnitComboFilterDto());
        }

        public async Task<List<ComboboxItemDto>> GetOrgUnitComboAsync(OrgUnitComboFilterDto dto)
        {
            var query = _repository.GetAll()
                .WhereIf(!dto.Filter.IsNullOrWhiteSpace(), p => p.Name.Contains(dto.Filter))
                .OrderBy(p => p.Name)
                .Select(p => new ComboboxItemDto
                {
                    Value = p.Id.ToString(),
                    Label = p.Name
                });

            return await query.ToListAsync();
        }

        public async Task<long> CreateOrgUnitAsync(OrgUnitDto dto)
        {
            dto.Level = await CalculateOrgUnitLevel(dto);
            await ValidateOrgUnitAsync(dto);

            var entity = new OrgUnit();

            entity.ParentOU = dto.ParentOU;
            entity.Name = dto.Name;
            entity.Level = dto.Level;

            entity.Id = _repository.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeOrgUnit(await GetOrgUnitAsync(entity.Id), null, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task UpdateOrgUnitAsync(OrgUnitDto dto)
        {
            await ValidateOrgUnitAsync(dto);

            var entity = await _repository.FirstOrDefaultAsync(dto.Id.Value);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("OrgUnits.OrgUnit"), dto.Id));
            }

            OrgUnitDto previousDto = await GetOrgUnitAsync(dto.Id.Value);

            entity.Name = dto.Name;

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeOrgUnit(await GetOrgUnitAsync(entity.Id), previousDto, ChangeLogType.Update);
        }

        public async Task DeleteOrgUnitAsync(long id)
        {
            OrgUnitDto previousDto = await GetOrgUnitAsync(id);

            if (previousDto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("OrgUnits.OrgUnit"), id));
            }

            List<long> childrens = await _repository.GetAll().Where(p => p.ParentOU == id).Select(p => p.Id).ToListAsync();

            foreach (long ou in childrens)
            {
                await DeleteOrgUnitAsync(ou);
            }

            await _repositoryOrgUnitUser.DeleteAsync(p => p.OrgUnit == id);
            await _repository.DeleteAsync(id);

            await LogChangeOrgUnit(null, previousDto, ChangeLogType.Delete);
        }

        #region Private Methods

        private IQueryable<OrgUnitDto> GetOrgUnitQuery()
        {
            var query = (from entity in _repository.GetAll()
                         select new OrgUnitDto
                         {
                             Id = entity.Id,
                             ParentOU = entity.ParentOU,
                             ParentOUDesc = entity.ParentOUNavigation.Name,
                             Name = entity.Name,
                             Level = entity.Level,
                             HasChildren = entity.InverseParentOUNavigation.Any(p => p.IsDeleted != true),
                             Size = entity.OrgUnitUser.Count
                         });

            return query;
        }

        private IQueryable<OrgUnitDto> GetOrgUnitListQuery(OrgUnitListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetOrgUnitQuery()
                .WhereIf(dto.ParentOU.HasValue,
                    p => p.ParentOU == dto.ParentOU
                ).WhereIf(dto.Level.HasValue,
                    p => p.Level == dto.Level
                ).WhereIf(
                    filter != null,
                    p => p.Id.ToString().Contains(filter)
                    || p.Name.ToUpper().Contains(filter)
                    || p.Level.ToString().Contains(filter)
                );

            return query;
        }

        private async Task ValidateOrgUnitAsync(OrgUnitDto dto)
        {
            List<OrgUnitDto> list = await GetOrgUnitByNameListAsync(dto.Name);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("OrgUnits.OrgUnit.DuplicatedName"));
            }

            if (dto.Id == null && dto.Level > 7)
            {
                throw new EntityDuplicatedException(L("OrgUnits.OrgUnit.MaxLevel"));
            }
        }

        private async Task<long> LogChangeOrgUnit(OrgUnitDto newDto, OrgUnitDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new OrgUnitDto(); }
            if (previousDto == null) { previousDto = new OrgUnitDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
				LogStringProperty(sb, previousDto.ParentOUDesc, newDto.ParentOUDesc, "{{OrgUnits.OrgUnit.ParentOU}}");
				LogStringProperty(sb, previousDto.Name, newDto.Name, "{{OrgUnits.OrgUnit.Name}}");
                LogIntProperty(sb, previousDto.Level, newDto.Level, "{{OrgUnits.OrgUnit.Level}}");
            }

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "OrgUnit", sb.ToString());
        }

        private async Task<byte> CalculateOrgUnitLevel(OrgUnitDto dto)
        {
            if (dto.ParentOU == null)
            {
                return 1;
            }
            else
            {
                return (byte) (await _repository.GetAll().Where(p => p.Id == dto.ParentOU).Select(p => p.Level).FirstOrDefaultAsync() + 1);
            }
        }

        #endregion

        #endregion

        #region Users

        public async Task<PagedResultDto<OrgUnitUserDto>> GetOrgUnitUserListAsync(OrgUnitUserListFilterDto dto)
        {
            var query = GetOrgUnitUserListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "UserDesc" : dto.Sorting)
                .PageBy(dto)
                .ToListAsync();

            return new PagedResultDto<OrgUnitUserDto>(count, ll);
        }

        public async Task<List<OrgUnitUserDto>> GetOrgUnitUserByOUListAsync(long ou)
        {
            var query = GetOrgUnitUserQuery();

            return await query.Where(p => p.OrgUnit == ou).ToListAsync();
        }

        public async Task<List<OrgUnitUserDto>> GetOrgUnitUserByOUAndUserListAsync(long ou, long user)
        {
            var query = GetOrgUnitUserQuery();

            return await query.Where(p => p.OrgUnit == ou && p.User == user).ToListAsync();
        }

        public async Task<long> CreateOrgUnitUserAsync(OrgUnitUserDto dto)
        {
            await ValidateOrgUnitUserAsync(dto);

            var entity = new OrgUnitUser();

            entity.OrgUnit = dto.OrgUnit;
            entity.User = dto.User;

            entity.Id = _repositoryOrgUnitUser.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateOrgUnitUserAsync(OrgUnitUserDto dto)
        {
            await ValidateOrgUnitUserAsync(dto);

            var entity = await _repositoryOrgUnitUser.FirstOrDefaultAsync(dto.Id.Value);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("OrgUnits.OrgUnit.User"), dto.Id));
            }

            entity.OrgUnit = dto.OrgUnit;
            entity.User = dto.User;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteOrgUnitUserAsync(long id)
        {
            var entity = await _repositoryOrgUnitUser.FirstOrDefaultAsync(id);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("OrgUnits.OrgUnit.User"), id));
            }

            await _repositoryOrgUnitUser.DeleteAsync(id);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        #region Private Methods

        private IQueryable<OrgUnitUserDto> GetOrgUnitUserQuery()
        {
            var query = (from entity in _repositoryOrgUnitUser.GetAll()
                         select new OrgUnitUserDto
                         {
                             Id = entity.Id,
                             OrgUnit = entity.OrgUnit,
                             OrgUnitDesc = entity.OrgUnitNavigation.Name,
                             User = entity.User,
                             UserDesc = ((entity.UserNavigation.Name + " " + entity.UserNavigation.Lastname).Trim() + " " + entity.UserNavigation.SecondLastname).Trim()
                         });

            return query;
        }

        private IQueryable<OrgUnitUserDto> GetOrgUnitUserListQuery(OrgUnitUserListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetOrgUnitUserQuery()
                .WhereIf(
                    dto.OrgUnit.HasValue,
                    p => p.OrgUnit == dto.OrgUnit
                ).WhereIf(
                    filter != null,
                    p => p.Id.ToString().Contains(filter)
                    || p.UserDesc.ToUpper().Contains(filter)
                );

            return query;
        }

        private async Task ValidateOrgUnitUserAsync(OrgUnitUserDto dto)
        {
            List<OrgUnitUserDto> list = await GetOrgUnitUserByOUAndUserListAsync(dto.OrgUnit, dto.User);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("OrgUnits.OrgUnit.DuplicatedUser"));
            }
        }

        #endregion

        #endregion
    }
}
