using AlgoriaCore.Application.Chat;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.Chat.Friendships.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Session;
using AlgoriaPersistence.Interfaces.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AlgoriaCore.Application.Managers.Chat.Friendships
{
    public class FriendshipManager : BaseManager, IFriendshipManager
    {
        private readonly IRepository<Friendship, long> _repFriendship;

        public FriendshipManager(
            IAppSession session,
            IUnitOfWork currentUnitOfWork, 
            IRepository<Friendship, long> repFriendship)
        {
            SessionContext = session;
            CurrentUnitOfWork = currentUnitOfWork;
            _repFriendship = repFriendship;
        }

        public long CreateFriendship(FriendshipDto dto)
        {
            if (dto.TenantId == dto.FriendTenantId && dto.UserId == dto.FriendUserId)
            {
                throw new AlgoriaCoreGeneralException(L("Friendship.YouCannotBeFriendWithYourself"));
            }

            var ant = GetFriendshipOrNull(dto);

            if (ant != null)
            {
                throw new AlgoriaCoreGeneralException(L("Friendship.AlreadyFriendshipExists"));
            }

            var entity = new Friendship();

            entity.UserId = dto.UserId;
            entity.FriendTenantId = dto.FriendTenantId;
            entity.FriendUserId = dto.FriendUserId;
            entity.CreationTime = Clock.Now;
            entity.FriendNickname = dto.FriendNickname;
            entity.State = (byte)dto.State;

            _repFriendship.Insert(entity);
            CurrentUnitOfWork.SaveChanges();

            return entity.Id;
        }

        public long UpdateFriendship(FriendshipDto dto)
        {
            if (SessionContext.TenantId == dto.FriendTenantId && SessionContext.UserId == dto.FriendUserId)
            {
                throw new AlgoriaCoreGeneralException(L("Friendship.YouCannotBeFriendWithYourself"));
            }

            var entity = _repFriendship.FirstOrDefault(m =>
                                    m.TenantId == SessionContext.TenantId && m.UserId == SessionContext.UserId.Value &&
                                    m.FriendTenantId == dto.FriendTenantId && m.FriendUserId == dto.FriendUserId);

            if (entity == null)
            {
                throw new AlgoriaCoreGeneralException(L("Friendship.NotFound"));
            }

            entity.FriendNickname = dto.FriendNickname;
            entity.State = (byte)dto.State;

            CurrentUnitOfWork.SaveChanges();

            return entity.Id;
        }

        public FriendshipDto GetFriendshipOrNull(FriendshipBaseDto dto)
        {
            using (CurrentUnitOfWork.SetTenantId(dto.TenantId))
            {
                var entity = _repFriendship.FirstOrDefault(m =>
                                        m.TenantId == dto.TenantId && m.UserId == dto.UserId &&
                                        m.FriendTenantId == dto.FriendTenantId && m.FriendUserId == dto.FriendUserId);

                return entity != null ? GetFriendship(entity) : null;
            }
        }

        public long AcceptFriendshipRequest(int? friendTenantId, long friendUserId)
        {
            return UpdateFriendshipState(friendTenantId, friendUserId, FriendshipState.Accepted);
        }

        public long BlockFriendship(int? friendTenantId, long friendUserId)
        {
            return UpdateFriendshipState(friendTenantId, friendUserId, FriendshipState.Blocked);
        }

        public List<FriendshipDto> GetFriendshipList(int? tenantId, long userId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var lista = new List<FriendshipDto>();
                var ll = _repFriendship.GetAll().Where(m => m.TenantId == tenantId && m.UserId == userId).ToList();

                ll.ForEach(entity => { lista.Add(GetFriendship(entity)); });

                return lista;
            }
        }

        public List<FriendshipDto> GetFriendshipList()
        {
            var lista = new List<FriendshipDto>();
            var ll = _repFriendship.GetAll().Where(m => m.UserId == SessionContext.UserId.Value).ToList();

            ll.ForEach(entity => { lista.Add(GetFriendship(entity)); });

            return lista;
        }

        #region Métodos privados

        private long UpdateFriendshipState(int? friendTenantId, long friendUserId, FriendshipState state)
        {
            var dto = GetFriendshipOrNull(new FriendshipBaseDto { TenantId = SessionContext.TenantId, UserId = SessionContext.UserId.Value, FriendTenantId = friendTenantId, FriendUserId = friendUserId });

            if (dto == null)
            {
                throw new AlgoriaCoreGeneralException(L("Friendship.NotFound"));
            }

            dto.State = state;

            return UpdateFriendship(dto);
        }

        private FriendshipDto GetFriendship(Friendship entity)
        {
            var dto = new FriendshipDto();

            dto.Id = entity.Id;
            dto.TenantId = entity.TenantId;
            dto.UserId = entity.UserId;
            dto.FriendTenantId = entity.FriendTenantId;
            dto.FriendUserId = entity.FriendUserId;
            dto.CreationTime = entity.CreationTime;
            dto.FriendNickname = entity.FriendNickname;
            dto.State = (FriendshipState)entity.State;

            return dto;
        }

        #endregion
    }
}