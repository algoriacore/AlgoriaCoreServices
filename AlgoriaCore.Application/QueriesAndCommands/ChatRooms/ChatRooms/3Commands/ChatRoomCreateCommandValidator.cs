using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomCreateCommandValidator : AbstractValidator<ChatRoomCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public ChatRoomCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelChatRoomId = L("ChatRooms.ChatRoom.ChatRoomId");
            string labelName = L("ChatRooms.ChatRoom.Name");
            string labelDescription = L("ChatRooms.ChatRoom.Description");

            RuleFor(x => x.ChatRoomId).NotEmpty().WithMessage(string.Format(labelRequiredField, labelChatRoomId));
            RuleFor(x => x.ChatRoomId).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelChatRoomId, 50));
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName));
            RuleFor(x => x.Name).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelName, 50));
            RuleFor(x => x.Description).NotEmpty().WithMessage(string.Format(labelRequiredField, labelDescription));
            RuleFor(x => x.Description).MaximumLength(250).WithMessage(string.Format(labelMaxLength, labelDescription, 250));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
