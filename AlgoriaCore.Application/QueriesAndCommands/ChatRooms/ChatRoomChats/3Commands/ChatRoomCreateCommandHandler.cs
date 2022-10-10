using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChatRooms;
using AlgoriaCore.Application.Managers.ChatRooms.Dto;
using AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats;
using AlgoriaCore.Extensions;
using MediatR;
using System;
using System.Linq;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRoomChats
{
    public class ChatRoomChatCreateCommandHandler : BaseCoreClass, IRequestHandler<ChatRoomChatCreateCommand, ChatRoomChatResponse>
    {
        private readonly ChatRoomManager _manager;

        public ChatRoomChatCreateCommandHandler(ICoreServices coreServices, ChatRoomManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<ChatRoomChatResponse> Handle(ChatRoomChatCreateCommand request, CancellationToken cancellationToken)
        {
            ChatRoomChatDto dto = new ChatRoomChatDto()
            {
                ChatRoom = request.ChatRoom,
                Comment = request.Comment,
                TaggedUsers = request.TaggedUsers.Select(p => new ChatRoomChatUserTaggedDto() { UserTagged = p }).ToList(),
                Files = request.Files.Select(p => new ChatRoomChatFileDto()
                {
                    FileName = p.FileName,
                    FileExtension = p.FileExtension,
                    Bytes = p.Base64.IsNullOrWhiteSpace() ? Array.Empty<byte>() : Convert.FromBase64String(p.Base64)
                }).ToList()
            };

            dto.Id = await _manager.CreateChatRoomChatAsync(dto);
            dto = await _manager.GetChatRoomChatAsync(dto.Id.Value);

            return new ChatRoomChatResponse()
            {
                Id = dto.Id.Value,
                ChatRoom = dto.ChatRoom,
                User = dto.User,
                UserDesc = dto.UserDesc,
                Comment = dto.Comment,
                CreationTime = dto.CreationTime,
                Files = dto.Files.Select(p => new ChatRoomChatFileResponse() {
                    FileName = p.FileName,
                    FileExtension = p.FileExtension,
                    File = p.File
                }).ToList()
            };
        }
    }
}
