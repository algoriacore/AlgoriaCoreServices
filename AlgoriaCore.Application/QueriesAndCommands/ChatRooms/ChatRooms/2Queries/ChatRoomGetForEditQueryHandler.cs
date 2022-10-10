﻿using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChatRooms;
using AlgoriaCore.Application.Managers.ChatRooms.Dto;
using AlgoriaCore.Application.Managers.Languages;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetForEditQueryHandler : BaseCoreClass, IRequestHandler<ChatRoomGetForEditQuery, ChatRoomForEditResponse>
    {
        private readonly ChatRoomManager _managerChatRoom;
        private readonly LanguageManager _managerLanguage;

        public ChatRoomGetForEditQueryHandler(ICoreServices coreServices, ChatRoomManager managerChatRoom, LanguageManager managerLanguage) : base(coreServices)
        {
            _managerChatRoom = managerChatRoom;
            _managerLanguage = managerLanguage;
        }

        public async Task<ChatRoomForEditResponse> Handle(ChatRoomGetForEditQuery request, CancellationToken cancellationToken)
        {
            ChatRoomForEditResponse response;
            List<ComboboxItemDto> languageComboDto = await _managerLanguage.GetLanguageCombo();

            if (request.Id.HasValue)
            {
                ChatRoomDto dto = await _managerChatRoom.GetChatRoomAsync(request.Id.Value);

                response = new ChatRoomForEditResponse()
                {
                    Id = dto.Id,
                    ChatRoomId = dto.ChatRoomId,
                    Name = dto.Name,
                    Description = dto.Description,
                    UserCreator = dto.UserCreator,
                    UserCreatorDesc = dto.UserCreatorDesc,
                    CreationTime = dto.CreationTime
                };
            }
            else
            {
                response = new ChatRoomForEditResponse();
            }

            return response;
        }
    }
}
