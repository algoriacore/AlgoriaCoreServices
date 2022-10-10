using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetByChatRoomIdValidator : AbstractValidator<ChatRoomGetByChatRoomIdQuery>
    {
        private readonly ICoreServices _coreServices;

        public ChatRoomGetByChatRoomIdValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelChatRoomId = L("ChatRooms.ChatRoom.ChatRoomId");

            RuleFor(x => x.ChatRoomId).NotEmpty().WithMessage(string.Format(labelRequiredField, labelChatRoomId));
            RuleFor(x => x.ChatRoomId).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelChatRoomId, 50));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
