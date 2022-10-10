using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands
{
    public class MailGroupCreateCommandValidator : AbstractValidator<MailGroupCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public MailGroupCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelDisplayName = L("EmailGroups.DisplayNameForm");

            RuleFor(x => x.DisplayName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelDisplayName));
            RuleFor(x => x.DisplayName).MaximumLength(100).WithMessage(string.Format(labelMaxLength, labelDisplayName, 100));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
