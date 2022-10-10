using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetForEditValidator : AbstractValidator<ChatRoomGetForEditQuery>
    {
        private readonly ICoreServices _coreServices;

        public ChatRoomGetForEditValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterOrEqualThanOrEmpty = L("FieldGreaterOrEqualThanOrEmpty");

            string labelId = L("Id");

            RuleFor(x => x.Id).Must(x => !x.HasValue || x > 0).WithMessage(string.Format(labelGreaterOrEqualThanOrEmpty, labelId, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
