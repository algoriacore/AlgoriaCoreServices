using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.Permissions.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Permissions
{
    public class PermissionManager : BaseManager
    {
        private readonly IRepository<Permission, long> _repository;
        private readonly IRepository<User, long> _repositoryUser;
        private readonly IRepository<UserRole, long> _repositoryUserRole;
        private readonly IRepository<Role, long> _repositoryRole;

        public PermissionManager(IRepository<Permission, long> repository
        , IRepository<User, long> repositoryUser
        , IRepository<UserRole, long> repositoryUserRole
        , IRepository<Role, long> repositoryRole)
        {
            _repository = repository;
            _repositoryUser = repositoryUser;
            _repositoryUserRole = repositoryUserRole;
            _repositoryRole = repositoryRole;
        }

        public async Task<bool> IsGrantedAsync(PermissionGetIsGrantedDto dto)
        {
            bool isGranted = true;

            if (dto.PermissionNames != null && dto.PermissionNames.Length > 0)
            {
                if (dto.RequiresAll)
                {
                    foreach (string permissionName in dto.PermissionNames)
                    {
                        if (!(await IsGrantedAsync(permissionName)))
                        {
                            isGranted = false;
                            break;
                        }
                    }
                }
                else
                {
                    isGranted = await IsGrantedAtLeastOneAsync(dto.PermissionNames);
                }
            }

            return isGranted;
        }

        public async Task<bool> IsGrantedAsync(string permissionName)
        {
            var query = (from U in _repositoryUser.GetAll()
                         join UR in _repositoryUserRole.GetAll() on U.Id equals UR.UserId
                         join R in _repositoryRole.GetAll() on UR.RoleId equals R.Id
                         join entidad in _repository.GetAll() on R.Id equals entidad.Role
                         where U.Id == SessionContext.UserId && entidad.Name == permissionName
                         select entidad);
            
            return await query.CountAsync() > 0;
        }

        public async Task<bool> IsGrantedAtLeastOneAsync(string[] permissionNames)
        {
            var query = (from U in _repositoryUser.GetAll()
                         join UR in _repositoryUserRole.GetAll() on U.Id equals UR.UserId
                         join R in _repositoryRole.GetAll() on UR.RoleId equals R.Id
                         join entidad in _repository.GetAll() on R.Id equals entidad.Role
                         where U.Id == SessionContext.UserId && permissionNames.Contains(entidad.Name)
                         select entidad);

            return await query.CountAsync() > 0;
        }

        public async Task<List<string>> GetPermissionListByUserAsync()
        {
            var lista = await (from U in _repositoryUser.GetAll()
                         join UR in _repositoryUserRole.GetAll() on U.Id equals UR.UserId
                         join R in _repositoryRole.GetAll() on UR.RoleId equals R.Id
                         join entidad in _repository.GetAll() on R.Id equals entidad.Role
                         where U.Id == SessionContext.UserId && R.IsActive == true
                         select entidad.Name).Distinct().ToListAsync();

            return lista;
        }
    }
}
