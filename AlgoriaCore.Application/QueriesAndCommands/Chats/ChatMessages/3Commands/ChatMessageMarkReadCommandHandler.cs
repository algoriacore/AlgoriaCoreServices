using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Chat.ChatMessages;
using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.ChatMessages._3Commands
{
    public class ChatMessageMarkReadCommandHandler : BaseCoreClass, IRequestHandler<ChatMessageMarkReadCommand, long>
    {
        private readonly ChatMessageManager _manager;

        public ChatMessageMarkReadCommandHandler(ICoreServices coreServices, ChatMessageManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(ChatMessageMarkReadCommand request, CancellationToken cancellationToken)
        {
            _manager.MarkAllUnreadMessagesOfUserAsRead(new ChatUser(request.FriendTenantId, request.FriendUserId));

            return await Task.FromResult(request.FriendUserId);
        }
    }
}
