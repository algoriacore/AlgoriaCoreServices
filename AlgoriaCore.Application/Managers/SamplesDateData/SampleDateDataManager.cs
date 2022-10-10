using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.SamplesDateData.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.SamplesDateData
{
    public class SampleDateDataManager : BaseManager
    {
        private readonly IRepository<SampleDateData, long> _repository;

        public SampleDateDataManager(IRepository<SampleDateData, long> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResultDto<SampleDateDataDto>> GetSampleDateDataListAsync(SampleDateDataListFilterDto dto)
        {
            var query = GetSampleDateDataListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "Name" : dto.Sorting)
                .PageBy(dto)
                .ToListAsync();

            return new PagedResultDto<SampleDateDataDto>(count, ll);
        }

        public async Task<SampleDateDataDto> GetSampleDateDataAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetSampleDateDataQuery();

            SampleDateDataDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Examples.DateTimes"), id));
            }

            return dto;
        }
        
        public async Task<long> CreateSampleDateDataAsync(SampleDateDataDto dto)
        {
            var entity = new SampleDateData();

            entity.TenantId = SessionContext.TenantId;
            entity.Name = dto.Name;
            entity.DateTimeData = dto.DateTimeData;
            entity.DateData = dto.DateData;
            entity.TimeData = dto.TimeData;

            entity.Id = _repository.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateSampleDateDataAsync(SampleDateDataDto dto)
        {
            var entity = await _repository.FirstOrDefaultAsync(dto.Id.Value);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Examples.DateTimes"), dto.Id));
            }

            entity.Name = dto.Name;
            entity.DateTimeData = dto.DateTimeData;
            entity.DateData = dto.DateData;
            entity.TimeData = dto.TimeData;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteSampleDateDataAsync(long id)
        {
            await _repository.DeleteAsync(id);

            CurrentUnitOfWork.SaveChanges();
        }

        #region Métodos privados

        private IQueryable<SampleDateDataDto> GetSampleDateDataQuery()
        {
            var query = (from entity in _repository.GetAll()
                         select new SampleDateDataDto
                         {
                             TenantId = entity.TenantId,
                             Id = entity.Id,
                             Name = entity.Name,
                             DateTimeData = entity.DateTimeData,
                             DateData = entity.DateData,
                             TimeData = entity.TimeData
                         });

            return query;
        }

        private IQueryable<SampleDateDataDto> GetSampleDateDataListQuery(SampleDateDataListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetSampleDateDataQuery()
                .WhereIf(
                    filter != null,
                    p => p.Id.ToString().Contains(filter)
                    || p.Name.ToUpper().Contains(filter)
                );

            return query;
        }

        #endregion
    }
}
