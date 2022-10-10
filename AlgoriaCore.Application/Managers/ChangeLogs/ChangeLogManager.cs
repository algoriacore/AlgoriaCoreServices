using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.ChangeLogs
{
    public class ChangeLogManager : BaseManager
    {
        private readonly IRepository<ChangeLog, long> _repository;
        private readonly IRepository<User, long> _repositoryUser;

        public ChangeLogManager(IRepository<ChangeLog, long> repository
        , IRepository<User, long> repositoryUser)
        {
            _repository = repository;
            _repositoryUser = repositoryUser;
        }

        public async Task<PagedResultDto<ChangeLogDto>> GetChangeLogListAsync(ChangeLogListFilterDto dto)
        {
            var query = GetChangeLogListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty()? "Id DESC": dto.Sorting)
                .PageBy(dto)
                .ToListAsync();

            return new PagedResultDto<ChangeLogDto>(count, ll);
        }

        public async Task<ChangeLogDto> GetChangeLogAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetChangeLogQuery();

            ChangeLogDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("ChangeLogs.ChangeLog"), id));
            }

            return dto;
        }

        public async Task<ChangeLogDto> GetChangeLogByTableAndKeyAsync(string table, string key)
        {
            var query = GetChangeLogQuery();

            ChangeLogDto dto = await query.FirstOrDefaultAsync(p => p.Table == table && p.Key == key);

            return dto;
        }

        public async Task<long> CreateChangeLogAsync(ChangeLogDto dto)
        {
            var entity = new ChangeLog();

            entity.UserId = SessionContext.UserId;
            entity.table = dto.Table;
            entity.key = dto.Key;
            entity.datetime = Clock.Now;

            if (!dto.Detail.IsNullOrEmpty())
            {
                entity.ChangeLogDetail.Add(new ChangeLogDetail() { data = dto.Detail });
            }

            entity.Id = _repository.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        #region Métodos privados

        private IQueryable<ChangeLogDto> GetChangeLogQuery()
        {
            var query = (from entity in _repository.GetAll()
                         join U in _repositoryUser.GetAll() on entity.UserId equals U.Id into UJoined
                         from U in UJoined.DefaultIfEmpty()
                         select new ChangeLogDto
                         {
                             Id = entity.Id,
                             User = entity.UserId,
                             UserDesc = (U.Name + " " + U.Lastname + (U.SecondLastname == null? "": " " + U.SecondLastname)).Trim(),
                             Table = entity.table,
                             Key = entity.key,
                             Datetime = entity.datetime,
                             Detail = entity.ChangeLogDetail.FirstOrDefault().data
                         });

            return query;
        }

        private IQueryable<ChangeLogDto> GetChangeLogListQuery(ChangeLogListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetChangeLogQuery()
                .WhereIf(
                    filter != null,
                    p => p.UserDesc.ToUpper().Contains(filter)
                    || p.Detail.ToUpper().Contains(filter)
                )
                .WhereIf(
                    !dto.Table.IsNullOrEmpty(),
                    p => p.Table == dto.Table
                ).WhereIf(
                    !dto.Key.IsNullOrEmpty(),
                    p => p.Key == dto.Key
                );

            return query;
        }

        #endregion
    }
}
