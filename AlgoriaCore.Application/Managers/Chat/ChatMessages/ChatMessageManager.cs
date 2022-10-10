using AlgoriaCore.Application.Chat;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using AlgoriaCore.Application.Managers.Chat.Friendships;
using AlgoriaCore.Application.Managers.Chat.Friendships.Dto;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaCore.Extensions.Utils;
using AlgoriaPersistence.Interfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoriaCore.Application.Managers.Chat.ChatMessages
{
    public class ChatMessageManager : BaseManager, IChatMessageManager
    {
        private readonly FriendshipManager _friendshipManager;
        private readonly IChatCommunicator _chatCommunicator;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly UserManager _userManager;
        private readonly IRepository<ChatMessage, long> _repChatMessage;
        
        public ChatMessageManager(
            IUnitOfWork currentUnitOfWork,
            IClock clock,
            IAppLocalizationProvider appLocalizationProvider,

            FriendshipManager friendshipManager,
            IChatCommunicator chatCommunicator,
            IOnlineClientManager onlineClientManager,
            UserManager userManager,
            IRepository<ChatMessage, long> repChatMessage            
            )
        {
            CurrentUnitOfWork = currentUnitOfWork;
            Clock = clock;
            AppLocalizationProvider = appLocalizationProvider;

            _friendshipManager = friendshipManager;
            _chatCommunicator = chatCommunicator;
            _onlineClientManager = onlineClientManager;
            _userManager = userManager;
            _repChatMessage = repChatMessage;
        }

        public void SendMessage(ChatUser sender, ChatUser receiver, string message, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId)
        {
            CheckReceiverExists(receiver);

            var friendshipState = _friendshipManager.GetFriendshipOrNull(new FriendshipBaseDto { TenantId = sender.TenantId, UserId = sender.UserId, FriendTenantId = receiver.TenantId, FriendUserId = receiver.UserId })?.State;
            if (friendshipState == FriendshipState.Blocked)
            {
                throw new AlgoriaCoreGeneralException(L("Friendship.UserIsBlocked"));
            }

            HandleSenderToReceiver(sender, receiver, message);
            HandleReceiverToSender(sender, receiver, message);
        }

        public List<ChatMessageDto> GetUserChatMessages(ChatMessageListFilterDto dto)
        {
            var lista = new List<ChatMessageDto>();

            var ll = _repChatMessage.GetAll()
                .WhereIf(dto.MinMessageId.HasValue, m => m.Id < dto.MinMessageId.Value)
                .Where(m => m.UserId == SessionContext.UserId && m.FriendTenantId == dto.TenantId && m.FriendUserId == dto.UserId)
                .OrderByDescending(m => m.CreationTime)
                .Take(50)
                .ToList();

            ll.Reverse();

            ll.ForEach(entity => { lista.Add(GetChatMessage(entity)); });

            return lista;
        }

        public void MarkAllUnreadMessagesOfUserAsRead(ChatUser friend)
        {
            var list = _repChatMessage.GetAll()
                .Where(m => m.UserId == SessionContext.UserId.Value &&
                        m.FriendTenantId == friend.TenantId &&
                        m.FriendUserId == friend.UserId &&
                        m.State == (byte)ChatMessageReadState.Unread)
                .ToList();

            if (!list.Any())
            {
                return;
            }

            foreach (var entity in list)
            {
                entity.State = (byte)ChatMessageReadState.Read;
            }

            CurrentUnitOfWork.SaveChanges();

            var userIdentifier = new ChatUser(SessionContext.TenantId, SessionContext.UserId.Value);

            var onlineClients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (onlineClients.Any())
            {
                _chatCommunicator.SendAllUnreadMessagesOfUserReadToClients(onlineClients, friend);
            }
        }

        public int GetUnreadMessagesCount(ChatUser friend)
        {
            var list = _repChatMessage.GetAll()
                .Where(m => m.UserId == SessionContext.UserId.Value &&
                        m.FriendTenantId == friend.TenantId &&
                        m.FriendUserId == friend.UserId &&
                        m.State == (byte)ChatMessageReadState.Unread)
                .ToList();

            return list.Count;
        }

        #region Métodos privados

        private void CheckReceiverExists(ChatUser receiver)
        {
            var receiverUser = AsyncUtil.RunSync(() => _userManager.GetUserOrNullAsync(receiver.TenantId, receiver.UserId));

            if (receiverUser == null)
            {
                throw new AlgoriaCoreGeneralException(L("Friendship.TargetUserNotFoundProbablyDeleted"));
            }
        }

        private void HandleSenderToReceiver(ChatUser sender, ChatUser receiver, string message)
        {
            var friendshipBaseDto = new FriendshipBaseDto { TenantId = sender.TenantId, UserId = sender.UserId, FriendTenantId = receiver.TenantId, FriendUserId = receiver.UserId };

            var friendshipState = _friendshipManager.GetFriendshipOrNull(friendshipBaseDto)?.State;
            if (friendshipState == null)
            {
                throw new AlgoriaCoreGeneralException(L("Friendship.NotFound"));
            }

            if (friendshipState.Value == FriendshipState.Blocked)
            {
                //Do not send message if receiver banned the sender
                return;
            }

            var sentMessage = new ChatMessageDto(
                sender,
                receiver,
                ChatSide.Sender,
                message,
                ChatMessageReadState.Read
            );

            Save(sentMessage);

            var chatMessageData = new ChatMessageData();
            chatMessageData.TenantId = sentMessage.TenantId;
            chatMessageData.UserId = sentMessage.UserId;
            chatMessageData.TargetTenantId = sentMessage.TargetTenantId;
            chatMessageData.TargetUserId = sentMessage.TargetUserId;
            chatMessageData.Message = sentMessage.Message;
            chatMessageData.CreationTime = sentMessage.CreationTime;
            chatMessageData.Side = sentMessage.Side;
            chatMessageData.ReadState = sentMessage.ReadState;

            _chatCommunicator.SendMessageToClient(_onlineClientManager.GetAllByUserId(sender), chatMessageData);
        }
        
        private void HandleReceiverToSender(ChatUser sender, ChatUser receiver, string message)
        {
            var friendshipBaseDto = new FriendshipBaseDto { TenantId = receiver.TenantId, UserId = receiver.UserId, FriendTenantId = sender.TenantId, FriendUserId = sender.UserId };

            var friendshipState = _friendshipManager.GetFriendshipOrNull(friendshipBaseDto)?.State;

            if (friendshipState == null)
            {
                throw new AlgoriaCoreGeneralException(L("Friendship.NotFound"));
            }

            if (friendshipState == FriendshipState.Blocked)
            {
                //Do not send message if receiver banned the sender
                return;
            }

            var sentMessage = new ChatMessageDto(
                    receiver,
                    sender,
                    ChatSide.Receiver,
                    message,
                    ChatMessageReadState.Unread);

            Save(sentMessage);

            var chatMessageData = new ChatMessageData();
            chatMessageData.TenantId = sentMessage.TenantId;
            chatMessageData.UserId = sentMessage.UserId;
            chatMessageData.TargetTenantId = sentMessage.TargetTenantId;
            chatMessageData.TargetUserId = sentMessage.TargetUserId;
            chatMessageData.Message = sentMessage.Message;
            chatMessageData.CreationTime = sentMessage.CreationTime;
            chatMessageData.Side = sentMessage.Side;
            chatMessageData.ReadState = sentMessage.ReadState;

            var clients = _onlineClientManager.GetAllByUserId(receiver);
            if (clients.Any())
            {
                _chatCommunicator.SendMessageToClient(clients, chatMessageData);
            }
        }

        private void Save(ChatMessageDto dto)
        {
            var entity = new ChatMessage();

            entity.TenantId = dto.TenantId;
            entity.UserId = dto.UserId;
            entity.FriendTenantId = dto.TargetTenantId;
            entity.FriendUserId = dto.TargetUserId;
            entity.CreationTime = Clock.Now;
            entity.Message = dto.Message;
            entity.State = (byte)dto.ReadState;
            entity.Side = (byte)dto.Side;

            using (CurrentUnitOfWork.SetTenantId(entity.TenantId))
            {
                _repChatMessage.Insert(entity);
                CurrentUnitOfWork.SaveChanges();
            }
        }

        private ChatMessageDto GetChatMessage(ChatMessage entity)
        {
            var dto = new ChatMessageDto();

            dto.Id = entity.Id;
            dto.TargetTenantId = entity.FriendTenantId;
            dto.TargetUserId = entity.FriendUserId;
            dto.CreationTime = entity.CreationTime.Value;
            dto.Message = entity.Message;
            dto.ReadState = (ChatMessageReadState)entity.State;
            dto.Side = (ChatSide)entity.Side;

            return dto;
        }

        #endregion
    }
}
