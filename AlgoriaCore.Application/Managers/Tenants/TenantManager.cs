using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.Tenants.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Tenants
{
    public class TenantManager : BaseManager
    {
        private readonly IRepository<Tenant, int> _repTenant;

        public TenantManager(IRepository<Tenant, int> repTenant)
        {
            _repTenant = repTenant;
        }

        public async Task<TenantDto> GetTenantByIdAsync(int id, bool throwExceptionIfNotFound = true)
        {
            TenantDto dto = null;
            var entity = await _repTenant.FirstOrDefaultAsync(m => m.Id == id);

            if (throwExceptionIfNotFound && entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Tenants.Tenant"), id));
            }

            if (entity != null)
            {
                dto = GetTenant(entity);
            }

            return dto;
        }

        public async Task<TenantDto> GetTenantByTenancyNameAsync(string tenancyName)
        {
            TenantDto dto = null;
            var entity = await _repTenant.FirstOrDefaultAsync(m => m.TenancyName == tenancyName);

            if (entity != null)
            {
                dto = GetTenant(entity);
            }

            return dto;
        }

        public async Task<int> CreateTenantAsync(TenantDto dto)
        {
            var ll = await _repTenant.FirstOrDefaultAsync(m => m.TenancyName == dto.TenancyName);

            if (ll != null)
            {
                throw new EntityDuplicatedException(dto.TenancyName);
            }

            ll = new Tenant();
            ll.TenancyName = dto.TenancyName;
            ll.Name = dto.Name;
            ll.CreationTime = Clock.Now;
            ll.IsActive = true;
            ll.IsDeleted = false;

            _repTenant.Insert(ll);
            CurrentUnitOfWork.SaveChanges();

            return ll.Id;
        }

        public async Task UpdateTenantAsync(TenantDto dto)
        {
            var entity = await _repTenant.FirstOrDefaultAsync(dto.Id);

            if(entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Tenants"), dto.Id));
            }

            entity.TenancyName = dto.TenancyName;
            entity.Name = dto.Name;
            entity.IsActive = dto.IsActive;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task<PagedResultDto<TenantDto>> GetTenantsListAsync(PageListByDto dto)
        {
            var a = GetTenantQuery()
                .WhereIf(!dto.Filter.IsNullOrEmpty(),
                        m => m.TenancyName.ToLower().Contains(dto.Filter.ToLower()) ||
						m.Id.ToString().Contains(dto.Filter.ToLower()) ||
                        m.Name.ToLower().Contains(dto.Filter.ToLower()) ||
                        m.IsActiveDesc.ToLower().Contains(dto.Filter.ToLower())
                );

            var tot = await a.CountAsync();
            var lst = await a.OrderBy(dto.Sorting)
                             .PageBy(dto)
                             .ToListAsync();

            return new PagedResultDto<TenantDto>(tot, lst);
        }

        public async Task<List<TenantDto>> GetTenantsListCompleterAsync(string filter)
        {
            var ll = await GetTenantQuery()
                .Where(m => m.IsActive && m.LargeName.ToUpper().Contains(filter.ToUpper()))
                .OrderBy(p => p.Name).ToListAsync();

            return ll;
        }

        public async Task<int> DeleteTenantAsync(int id)
        {
            var dto = await GetTenantByIdAsync(id);

            if (dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Tenants"), id));
            }

            await _repTenant.DeleteAsync(id);

            await CurrentUnitOfWork.SaveChangesAsync();

            return id;
        }

        #region Métodos privados

        private TenantDto GetTenant(Tenant entity)
        {
            var dto = new TenantDto();
            dto.Id = entity.Id;
            dto.TenancyName = entity.TenancyName;
            dto.Name = entity.Name;
            dto.CreationTime = entity.CreationTime;
            dto.IsActive = entity.IsActive ?? true;
            dto.IsDeleted = entity.IsDeleted ?? false;

            return dto;
        }

        private IQueryable<TenantDto> GetTenantQuery()
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");

            var query = (from t in _repTenant.GetAll()
                         select new TenantDto
                         {
                             Id = t.Id,
                             TenancyName = t != null ? t.TenancyName : null,
                             Name = t.Name,
                             LargeName = t.TenancyName + " - " + t.Name,
                             CreationTime = t.CreationTime,
                             IsActive = t.IsActive == true,
                             IsActiveDesc = t.IsActive == true ? yesLabel : noLabel,
                             IsDeleted = t.IsDeleted == true
                         });
            return query;
        }

        #endregion
    }
}
