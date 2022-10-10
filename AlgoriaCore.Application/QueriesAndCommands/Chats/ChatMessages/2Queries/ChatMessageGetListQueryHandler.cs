using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Chat.ChatMessages;
using AlgoriaCore.Application.QueriesAndCommands.Chats.ChatMessages._1Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.ChatMessages._2Queries
{
    public class ChatMessageGetListQueryHandler : BaseCoreClass, IRequestHandler<ChatMessageGetListQuery, PagedResultDto<ChatMessageListResponse>>
    {
        private readonly ChatMessageManager _manager;

        public ChatMessageGetListQueryHandler(ICoreServices coreServices, ChatMessageManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<ChatMessageListResponse>> Handle(ChatMessageGetListQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<ChatMessageListResponse>();

            var ll = _manager.GetUserChatMessages(new Managers.Chat.ChatMessages.Dto.ChatMessageListFilterDto { TenantId = request.TenantId, UserId = request.UserId, MinMessageId = request.MinMessageId });

            ll.ForEach(dto => {
                lista.Add(new ChatMessageListResponse {
                    Id = dto.Id.Value,
                    TargetTenantId = dto.TargetTenantId,
                    TargetUserId = dto.TargetUserId,
                    CreationTime = dto.CreationTime,
                    Message = dto.Message,
                    ReadState = dto.ReadState,
                    Side = dto.Side
                });
            });

            return await Task.FromResult(new PagedResultDto<ChatMessageListResponse>(lista.Count, lista));
        }
    }
}
