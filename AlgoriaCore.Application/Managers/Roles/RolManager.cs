using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.Roles.Dto;
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

namespace AlgoriaCore.Application.Managers.Roles
{
    public class RolManager : BaseManager
    {
        private readonly IRepository<Role, long> _repRol;
        private readonly IRepository<Permission, long> _repPermission;

        public RolManager(IRepository<Role, long> repRol,
                          IRepository<Permission, long> repPermission)
        {
            _repRol = repRol;
            _repPermission = repPermission;
        }

        public async Task<PagedResultDto<RolDto>> GetRolesListAsync(RolListFilterDto dto)
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");

            var a = (from r in _repRol.GetAll()
                     select new RolDto
                     {
                         Id = r.Id,
                         Name = r.Name,
                         DisplayName = r.DisplayName,
                         IsActive = r.IsActive,
                         IsActiveDesc = r.IsActive == true ? yesLabel : noLabel,
                         IsDeleted = r.IsDeleted
                     }).WhereIf(!dto.Filter.IsNullOrEmpty(),
                        m => m.DisplayName.ToLower().Contains(dto.Filter.ToLower())
                        || m.Name.ToLower().Contains(dto.Filter.ToLower())
                        || m.IsActiveDesc.ToLower().Contains(dto.Filter.ToLower())
                        || m.Id.ToString().Contains(dto.Filter.ToLower())
                     );

            var tot = await a.CountAsync();
            var lst = await a.OrderBy(dto.Sorting)
                     .PageBy(dto)
                     .ToListAsync();

            return new PagedResultDto<RolDto>(tot, lst);
        }

        public async Task<List<RolDto>> GetRolListAsync()
        {
            var ll = await _repRol.GetAll().ToListAsync();

            var lista = new List<RolDto>();

            ll.ForEach(m => lista.Add(GetRol(m)));

            return lista;
        }

        public async Task<List<RolDto>> GetRolListActiveAsync()
        {
            var ll = await GetRolListAsync();

            ll = ll.Where(m => m.IsActive == true).ToList();

            return ll;
        }

        public async Task<RolDto> GetRoleByIdAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var r = await _repRol.FirstOrDefaultAsync(id);

            if (throwExceptionIfNotFound && r == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Roles"), id));
            }

            RolDto rolDto = GetRol(r);

            return rolDto;
        }

        public async Task<long> AddRolAsync(RolDto dto, List<string> permissionNames)
        {
            var entity = new Role
            {
                TenantId = dto.TenantId,
                Name = dto.Name,
                DisplayName = dto.DisplayName,
                IsActive = dto.IsActive,
                IsDeleted = dto.IsDeleted
            };

            _repRol.Insert(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChange(await GetRoleByIdAsync(entity.Id), null, permissionNames, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task UpdateRolAsync(RolDto dto, List<string> permissionNames)
        {
            var entity = await _repRol.FirstOrDefaultAsync(dto.Id ?? 0);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Roles"), dto.Id));
            }

            var previousDto = await GetRoleByIdAsync(dto.Id.Value);

            entity.Name = dto.Name;
            entity.DisplayName = dto.DisplayName;
            entity.IsActive = dto.IsActive;
            entity.IsDeleted = dto.IsDeleted;

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChange(await GetRoleByIdAsync(entity.Id), previousDto, permissionNames, ChangeLogType.Update);
        }

        public async Task<long> DeleteRolAsync(long id)
        {
            var rolDto = await GetRoleByIdAsync(id);

            if (rolDto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Roles"), id));
            }

            await _repRol.DeleteAsync(id);

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChange(null, rolDto, null, ChangeLogType.Delete);

            return id;
        }

        public async Task<List<RolDto>> GetRolesFromNamesByValidating(List<string> names)
        {
            var ll = new List<RolDto>();
            var undefinedNames = new List<string>();
            var rolList = await GetRolListAsync();

            foreach (var name in names)
            {
                var rol = rolList.FirstOrDefault(m => m.Name == name);
                if (rol == null)
                {
                    undefinedNames.Add(name);
                }
                else
                {
                    ll.Add(rol);
                }
            }

            if (undefinedNames.Count > 0)
            {
                throw new AlgoriaCoreGeneralException(string.Format(L("Roles.NotDefinedFound"), undefinedNames.Count));
            }

            return ll;
        }

        #region Métodos privados

        private static RolDto GetRol(Role entity)
        {
            var dto = new RolDto();

            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.DisplayName = entity.DisplayName;
            dto.IsActive = entity.IsActive;
            dto.IsDeleted = entity.IsDeleted;

            return dto;
        }

        private async Task<long> LogChange(RolDto newDto, RolDto previousDto, List<string> newPermissionNames, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new RolDto(); }
            if (previousDto == null) { previousDto = new RolDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
				LogStringProperty(sb, previousDto.Name, newDto.Name, "{{Roles.NameForm}}");
				LogStringProperty(sb, previousDto.DisplayName, newDto.DisplayName, "{{Roles.DisplayNameForm}}");
				
                if (newDto.IsActive != previousDto.IsActive)
                {
                    sb.AppendFormat("{0}: {1} => {2}\n", "{{Status}}", previousDto.IsActive == true ? "{{Active}}" : "{{Inactive}}", newDto.IsActive == true ? "{{Active}}" : "{{Inactive}}");
                }

                if (newPermissionNames != null)
                {
                    sb.AppendFormat("{0}: {1}", "{{Roles.PermissionForm}}", string.Join(',', newPermissionNames.ToArray()));
                }
            }

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "Role", sb.ToString());
        }

        #endregion

        #region Permissions

        public async Task<List<PermissionDto>> GetAllPermissionsByRoleIdAsync(long roleId)
        {
            var lista = new List<PermissionDto>();

            var ll = await _repPermission.GetAll().Where(m => m.Role == roleId).ToListAsync();

            foreach (var entity in ll)
            {
                lista.Add(new PermissionDto
                {
                    Id = entity.Id,
                    RoleId = entity.Role.Value,
                    Name = entity.Name
                });
            }

            return lista;
        }

        public async Task ReplacePermissionAsync(long roleId, List<PermissionDto> permissions)
        {
            var p = _repPermission.GetAll().Where(m => m.Role == roleId).ToList();

            foreach (var r in p)
            {
                await _repPermission.DeleteAsync(r);
            }
            await CurrentUnitOfWork.SaveChangesAsync();

            foreach (var ma in permissions.Select(m => m.Name).Distinct())
            {
                var pr = new Permission
                {
                    Role = roleId,
                    Name = ma,
                    IsGranted = true
                };

                await _repPermission.InsertAsync(pr);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        #endregion
    }
}
