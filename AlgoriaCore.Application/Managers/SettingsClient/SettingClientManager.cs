using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.SettingsClient.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.SettingsClient
{
    public class SettingClientManager : BaseManager
    {
        private readonly IRepository<SettingClient, long> _repository;

        public SettingClientManager(IRepository<SettingClient, long> repository)
        {
            _repository = repository;
        }

        public async Task<List<SettingClientDto>> GetSettingClientByClientTypeAndUserLogged(string clientType)
        {
            return await GetSettingClientByClientTypeAndUser(clientType, SessionContext.UserId.Value);
        }

        public async Task<List<SettingClientDto>> GetSettingClientByClientTypeAndUser(string clientType, long user) {
            var query = GetSettingClientQuery();

            return await query.Where(p => p.ClientType == clientType && p.User == user).ToListAsync();
        }

        public async Task<SettingClientDto> GetSettingClient(string clientType, long user, string name)
        {
            var query = GetSettingClientQuery();

            return await query.FirstOrDefaultAsync(p => p.ClientType == clientType && p.User == user && p.Name.ToLower() == name.ToLower());
        }

        public async Task<long> ChangeSettingClient(SettingClientDto dto)
        {
            var settingDto = await GetSettingClient(dto.ClientType, dto.User.Value, dto.Name);

            if (settingDto == null)
            {
                return await CreateSettingClient(dto);
            }
            else
            {
                return await UpdateSettingClient(dto);
            }
        }

        public async Task ChangeSettingClient(List<SettingClientDto> list)
        {
            foreach(SettingClientDto dto in list)
            {
                await ChangeSettingClient(dto);
            }
        }

        public async Task ChangeSettingClient(string clientType, long user, Dictionary<string, string> settings)
        {
            foreach (KeyValuePair<string, string> entry in settings)
            {
                await ChangeSettingClient(new SettingClientDto()
                {
                    ClientType = clientType,
                    User = user,
                    Name = entry.Key,
                    Value = entry.Value
                });
            }
        }

        #region Métodos privados

        private IQueryable<SettingClientDto> GetSettingClientQuery()
        {
            var query = (from entity in _repository.GetAll()
                         select new SettingClientDto
                         {
                             Id = entity.Id,
                             User = entity.UserId,
                             ClientType = entity.ClientType,
                             Name = entity.Name,
                             Value = entity.value
                         });

            return query;
        }

        private async Task<long> CreateSettingClient(SettingClientDto dto)
        {
            var entity = await _repository.FirstOrDefaultAsync(m => m.ClientType == dto.ClientType && m.UserId == dto.User && m.Name.ToLower() == dto.Name.ToLower());

            if (entity != null)
            {
                throw new EntityDuplicatedException(string.Format(L("EntityDuplicatedExceptionMessage"), L("SettingClient"), dto.Name));
            }

            entity = new SettingClient() { ClientType = dto.ClientType, UserId = dto.User, Name = dto.Name, value = dto.Value };

            _repository.Insert(entity);
            CurrentUnitOfWork.SaveChanges();

            return entity.Id;
        }

        private async Task<long> UpdateSettingClient(SettingClientDto dto)
        {
            var entity = await _repository.FirstOrDefaultAsync(m => m.ClientType == dto.ClientType && m.UserId == dto.User && m.Name.ToLower() == dto.Name.ToLower());

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("SettingClient"), dto.Name));
            }

            entity.value = dto.Value;
            CurrentUnitOfWork.SaveChanges();

            return entity.Id;
        }

        #endregion
    }
}
