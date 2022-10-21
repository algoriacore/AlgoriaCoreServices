using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRoomChats
{
    public class ChatRoomChatCreateCommandValidator : AbstractValidator<ChatRoomChatCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public ChatRoomChatCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterThan = L("FieldGreaterThan");
            string labelRequiredField = L("RequiredField");

            string labelChatRoom = L("ChatRoomChats.ChatRoomChat.ChatRoom");
            string labelComment = L("ChatRoomChats.ChatRoomChat.Comment");

            RuleFor(x => x.ChatRoom).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelChatRoom, 0));
            RuleFor(x => x.Comment).NotEmpty().WithMessage(string.Format(labelRequiredField, labelComment));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
