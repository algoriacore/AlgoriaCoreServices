using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Chat;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using AlgoriaCore.Application.Managers.Chat.Friendships;
using AlgoriaCore.Application.Managers.Chat.Friendships.Dto;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaPersistence.Interfaces.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._3Commands
{
    public class FriendshipCreateCommandHandler : BaseCoreClass, IRequestHandler<FriendshipCreateCommand, long>
    {
        private readonly FriendshipManager _manager;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly UserManager _userManager;
        private readonly IUnitOfWork _currentUnitOfWork;
        private readonly IChatCommunicator _chatCommunicator;

        public FriendshipCreateCommandHandler(
            ICoreServices coreServices, 
            FriendshipManager manager,
            IOnlineClientManager onlineClientManager,
            UserManager userManager,
            IUnitOfWork currentUnitOfWork,
            IChatCommunicator chatCommunicator
            ) : base(coreServices)
        {
            _manager = manager;
            _onlineClientManager = onlineClientManager;
            _userManager = userManager;
            _currentUnitOfWork = currentUnitOfWork;
            _chatCommunicator = chatCommunicator;
        }

        public async Task<long> Handle(FriendshipCreateCommand request, CancellationToken cancellationToken)
        {
            var friendshipBaseDto = new FriendshipBaseDto { TenantId = SessionContext.TenantId, UserId = SessionContext.UserId.Value, FriendTenantId = request.FriendTenantId, FriendUserId = request.FriendUserId };

            if (_manager.GetFriendshipOrNull(friendshipBaseDto) != null)
            {
                throw new AlgoriaCoreGeneralException(L("Friendship.AlreadyFriendshipExists"));
            }

            UserDto userDto = await _userManager.GetUserOfSessionAsync();
            UserDto userFriendDto = null;

            using (_currentUnitOfWork.SetTenantId(request.FriendTenantId))
            {
                userFriendDto = await _userManager.GetUserById(request.FriendUserId);
            }

            var sourceFriendship = new FriendshipDto { TenantId = SessionContext.TenantId, UserId = SessionContext.UserId.Value, FriendTenantId = request.FriendTenantId, FriendUserId = request.FriendUserId, FriendNickname = userFriendDto.FullName, State = FriendshipState.Accepted };
            sourceFriendship.Id = _manager.CreateFriendship(sourceFriendship);

            var targetFriendship = new FriendshipDto { TenantId = request.FriendTenantId, UserId = request.FriendUserId, FriendTenantId = SessionContext.TenantId, FriendUserId = SessionContext.UserId.Value, FriendNickname = userDto.FullName, State = FriendshipState.Accepted };
            using (_currentUnitOfWork.SetTenantId(request.FriendTenantId))
            {
                targetFriendship.Id = _manager.CreateFriendship(targetFriendship);
            }

            var targetChatUser = new ChatUser(request.FriendTenantId, request.FriendUserId);
            var sourceChatUser = new ChatUser(SessionContext.TenantId, SessionContext.UserId.Value);

            var targetClients = _onlineClientManager.GetAllByUserId(targetChatUser);

            if (targetClients.Any())
            {
                var chatFriendData = new ChatFriendData();

                chatFriendData.Id = targetFriendship.Id;
                chatFriendData.FriendTenantId = targetFriendship.FriendTenantId;
                chatFriendData.FriendNickname = targetFriendship.FriendNickname;
                chatFriendData.State = targetFriendship.State;
                chatFriendData.IsOnline = _onlineClientManager.IsOnline(sourceChatUser);

                _chatCommunicator.SendFriendshipRequestToClient(targetClients, chatFriendData, false);
            }

            var sourceClients = _onlineClientManager.GetAllByUserId(sourceChatUser);

            if (sourceClients.Any())
            {
                var chatFriendData = new ChatFriendData();

                chatFriendData.Id = sourceFriendship.Id;
                chatFriendData.FriendTenantId = sourceFriendship.FriendTenantId;
                chatFriendData.FriendNickname = sourceFriendship.FriendNickname;
                chatFriendData.State = sourceFriendship.State;
                chatFriendData.IsOnline = _onlineClientManager.IsOnline(targetChatUser);

                _chatCommunicator.SendFriendshipRequestToClient(sourceClients, chatFriendData, true);
            }

            return await Task.FromResult(sourceFriendship.Id.Value);
        }
    }
}
