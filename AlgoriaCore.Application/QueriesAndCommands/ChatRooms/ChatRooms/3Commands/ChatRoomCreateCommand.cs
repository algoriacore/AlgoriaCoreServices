﻿using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomCreateCommand : IRequest<long>
    {
        public string ChatRoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}