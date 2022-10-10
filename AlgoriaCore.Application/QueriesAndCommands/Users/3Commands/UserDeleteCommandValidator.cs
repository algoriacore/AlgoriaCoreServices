using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserDeleteCommandValidator : AbstractValidator<UserDeleteCommand>
    {
        private readonly ICoreServices _coreServices;

        public UserDeleteCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterThan = L("FieldGreaterThan");

            string labelId = L("Id");

            RuleFor(x => x.Id).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelId, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
