using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensiones;
using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.BinaryObjects;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.ChatRooms.Dto;
using AlgoriaCore.Application.Managers.Emails.Templates;
using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using AlgoriaCore.Domain.Email;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.ChatRooms
{
    public class ChatRoomManager : BaseManager
    {
        private readonly IRepository<ChatRoom, long> _repository;
        private readonly IRepository<ChatRoomChat, long> _repositoryChatRoomChat;

        private readonly BinaryObjectManager _managerBinaryObject;
        private readonly MailTemplateManager _mailTemplateManager;

        public ChatRoomManager(IRepository<ChatRoom, long> repository, IRepository<ChatRoomChat, long> repositoryChatRoomChat, 
            BinaryObjectManager managerBinaryObject, MailTemplateManager mailTemplateManager)
        {
            _repository = repository;
            _repositoryChatRoomChat = repositoryChatRoomChat;

            _managerBinaryObject = managerBinaryObject;
            _mailTemplateManager = mailTemplateManager;
        }

        #region CHAT ROOMS

        public async Task<PagedResultDto<ChatRoomDto>> GetChatRoomListAsync(ChatRoomListFilterDto dto)
        {
            var query = GetChatRoomListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "Name" : dto.Sorting)
                .PageBy(dto)
                .ToListAsync();

            return new PagedResultDto<ChatRoomDto>(count, ll);
        }

        public async Task<ChatRoomDto> GetChatRoomAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetChatRoomQuery();

            ChatRoomDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("ChatRooms.ChatRoom"), id));
            }

            return dto;
        }

        public async Task<List<ChatRoomDto>> GetChatRoomByChatRoomIdListAsync(string chatRoomId)
        {
            var query = GetChatRoomQuery();

            return await query.Where(p => p.ChatRoomId == chatRoomId).ToListAsync();
        }

        public async Task<long> CreateChatRoomAsync(ChatRoomDto dto)
        {
            await ValidateChatRoomAsync(dto);

            var entity = new ChatRoom();

            entity.ChatRoomId = dto.ChatRoomId;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.UserCreator = SessionContext.UserId;
            entity.CreationTime = DateTime.UtcNow;

            entity.Id = _repository.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeChatRoom(await GetChatRoomAsync(entity.Id), null, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task UpdateChatRoomAsync(ChatRoomDto dto)
        {
            await ValidateChatRoomAsync(dto);

            ChatRoomDto previousDto = await GetChatRoomAsync(dto.Id.Value);
            var entity = await _repository.FirstOrDefaultAsync(dto.Id.Value);

            entity.Name = dto.Name;
            entity.Description = dto.Description;

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeChatRoom(await GetChatRoomAsync(entity.Id), previousDto, ChangeLogType.Update);
        }

        #region Private Methods

        private IQueryable<ChatRoomDto> GetChatRoomQuery()
        {
            var query = (from entity in _repository.GetAll()
                         select new ChatRoomDto
                         {
                             Id = entity.Id,
                             ChatRoomId = entity.ChatRoomId,
                             Name = entity.Name,
                             Description = entity.Description,
                             UserCreator = entity.UserCreator,
                             UserCreatorDesc = ((entity.UserCreatorNavigation.Name + " " + entity.UserCreatorNavigation.Lastname).Trim() + " " +
                                entity.UserCreatorNavigation.SecondLastname).Trim(),
                             CreationTime = entity.CreationTime
                         });

            return query;
        }

        private IQueryable<ChatRoomDto> GetChatRoomListQuery(ChatRoomListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetChatRoomQuery()
                .WhereIf(
                    filter != null,
                    p => p.Id.ToString().ToUpper().Contains(filter)
                    || p.ChatRoomId.ToUpper().Contains(filter)
                    || p.Name.ToUpper().Contains(filter)
                    || p.Description.ToUpper().Contains(filter)
                    || p.UserCreatorDesc.ToUpper().Contains(filter)
                );

            return query;
        }

        private async Task ValidateChatRoomAsync(ChatRoomDto dto)
        {
            List<ChatRoomDto> list = await GetChatRoomByChatRoomIdListAsync(dto.ChatRoomId);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("ChatRooms.ChatRoom.DuplicatedChatRoomId"));
            }
        }

        private async Task<long> LogChangeChatRoom(ChatRoomDto newDto, ChatRoomDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new ChatRoomDto(); }
            if (previousDto == null) { previousDto = new ChatRoomDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
                LogStringProperty(sb, previousDto.ChatRoomId, newDto.ChatRoomId, "{{ChatRooms.ChatRoom.ChatRoomId}}");
                LogStringProperty(sb, previousDto.Name, newDto.Name, "{{ChatRooms.ChatRoom.Name}}");
                LogStringProperty(sb, previousDto.Description, newDto.Description, "{{ChatRooms.ChatRoom.Description}}");
            }

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "ChatRoom", sb.ToString());
        }

        #endregion

        #endregion

        #region MESSAGES

        public async Task<PagedResultDto<ChatRoomChatDto>> GetChatRoomChatListAsync(ChatRoomChatListFilterDto dto)
        {
            var query = GetChatRoomChatListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "CreationTime" : dto.Sorting)
                .Skip(dto.Skip)
                .Take(dto.PageSize.Value)
                //.PageBy(dto)
                .ToListAsync();

            return new PagedResultDto<ChatRoomChatDto>(count, ll);
        }

		public async Task<List<ChatRoomChatDto>> GetChatRoomChatListForLogAsync(ChatRoomChatForLogFilterDto dto)
		{
            var query = await GetChatRoomChatQuery()
                        .WhereIf(dto.LastId > 0, p => p.Id < dto.LastId)
                        .OrderByDescending(m => m.CreationTime)
                        .Take(10)
                        .ToListAsync();

            return query;
		}


		public async Task<ChatRoomChatDto> GetChatRoomChatAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetChatRoomChatQuery();

            ChatRoomChatDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("ChatRoomChats.ChatRoomChat"), id));
            }

            return dto;
        }

        public async Task<long> CreateChatRoomChatAsync(ChatRoomChatDto dto)
        {
            var entity = new ChatRoomChat();

            entity.ChatRoom = dto.ChatRoom;
            entity.User = SessionContext.UserId;
            entity.CreationTime = DateTime.UtcNow;
            entity.Comment = dto.Comment;

            if (dto.TaggedUsers.Count > 0) {
                foreach(ChatRoomChatUserTaggedDto chatRoomChatUserTaggedDto in dto.TaggedUsers) {
                    entity.ChatRoomChatUserTagged.Add(new ChatRoomChatUserTagged() { UserTagged = chatRoomChatUserTaggedDto.UserTagged });
                }
            }

            foreach(ChatRoomChatFileDto fileDto in dto.Files) 
            {
                if (fileDto.Bytes != null && fileDto.Bytes.Length > 0)
                {
                    entity.ChatRoomChatFile.Add(new ChatRoomChatFile()
                    {
                        FileName = fileDto.FileName,
                        FileExtension = fileDto.FileExtension,
                        File = await _managerBinaryObject.CreateAsync(fileDto.Bytes)
                    });
                }
            }

            entity.Id = _repositoryChatRoomChat.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            if (dto.TaggedUsers.Count > 0)
            {
                await SendTaggedUserEmailAsync(await GetChatRoomAsync(dto.ChatRoom), await GetChatRoomChatAsync(entity.Id));
            }

            return entity.Id;
        }

        #region Private Methods

        private IQueryable<ChatRoomChatDto> GetChatRoomChatQuery()
        {
            var query = (from entity in _repositoryChatRoomChat.GetAll()
                         select new ChatRoomChatDto
                         {
                             Id = entity.Id,
                             ChatRoom = entity.ChatRoom,
                             ChatRoomId = entity.ChatRoomNavigation.ChatRoomId,
                             User = entity.User,
                             UserDesc = ((entity.UserNavigation.Name + " " + entity.UserNavigation.Lastname).Trim() + " " +
                                entity.UserNavigation.SecondLastname).Trim(),
                             UserLogin = entity.UserNavigation.UserLogin,
                             CreationTime = entity.CreationTime,
                             Comment = entity.Comment,
                             TaggedUsers = entity.ChatRoomChatUserTagged.Select(p => new ChatRoomChatUserTaggedDto()
                             {
                                 UserTagged = p.UserTagged,
                                 UserTaggedDesc = ((p.UserTaggedNavigation.Name + " " + p.UserTaggedNavigation.Lastname).Trim() + " " +
                                    p.UserTaggedNavigation.SecondLastname).Trim(),
                                 UserTaggedEmail = p.UserTaggedNavigation.EmailAddress
                             }).ToList(),
                             Files = entity.ChatRoomChatFile.Select(p => new ChatRoomChatFileDto() {
                                 FileName = p.FileName,
                                 FileExtension = p.FileExtension,
                                 File = p.File
                             }).ToList(),
                             HasFiles = entity.ChatRoomChatFile.Count > 0
                         });

            return query;
        }

        private IQueryable<ChatRoomChatDto> GetChatRoomChatListQuery(ChatRoomChatListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetChatRoomChatQuery()
                .WhereIf(dto.ChatRoom != null,
                    p => p.ChatRoom == dto.ChatRoom
                ).WhereIf(dto.ChatRoomId != null,
                    p => p.ChatRoomId == dto.ChatRoomId
                ).WhereIf(dto.OnlyFiles,
                    p => p.HasFiles
                ).WhereIf(
                    filter != null,
                    p => p.UserDesc.ToUpper().Contains(filter)
                    || p.Comment.ToUpper().Contains(filter)
                );

            return query;
        }

        public async Task SendTaggedUserEmailAsync(ChatRoomDto chatRoomDto, ChatRoomChatDto dto)
        {
            try
            {
                MailTemplateDto emailTemplateDto = await _mailTemplateManager.GetMailTemplateCurrentByMailKey(EmailKeys.ChatTaggedUser);

                foreach (ChatRoomChatUserTaggedDto chatRoomChatUserTaggedDto in dto.TaggedUsers)
                {
                    var emailAddress = new EmailAddress { Address = chatRoomChatUserTaggedDto.UserTaggedEmail, Name = chatRoomChatUserTaggedDto.UserTaggedDesc };

                    var dict = new Dictionary<string, string>();

                    dict.Add(EmailVariables.TagChatUser.ChatRoom.ToParam(), chatRoomDto.Name);
                    dict.Add(EmailVariables.TagChatUser.ChatRoomDescription.ToParam(), chatRoomDto.Description);
                    dict.Add(EmailVariables.TagChatUser.TaggedUser.ToParam(), chatRoomChatUserTaggedDto.UserTaggedDesc);
                    dict.Add(EmailVariables.TagChatUser.TaggedBy.ToParam(), dto.UserDesc);
                    dict.Add(EmailVariables.TagChatUser.TaggedDateTime.ToParam(), dto.CreationTime.GetDateTimeISO());
                    dict.Add(EmailVariables.TagChatUser.Comment.ToParam(), dto.Comment);

                    SendEmail(emailTemplateDto, emailAddress, dict);
                }
            } catch (Exception ex) {
                CoreLogger.LogWarning(ex, ex.Message);
            }
        }

        #endregion

        #endregion
    }
}
