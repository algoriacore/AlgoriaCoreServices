using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpCreateCommandValidator : AbstractValidator<HelpCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public HelpCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelLanguage = L("Helps.Help.Language");
            string labelKey = L("Helps.Help.Key");
            string labelDisplayName = L("Helps.Help.DisplayName");
            string labelBody = L("Body");

            RuleFor(x => x.Language).NotEmpty().WithMessage(string.Format(labelRequiredField, labelLanguage));
            RuleFor(x => x.Key).NotEmpty().WithMessage(string.Format(labelRequiredField, labelKey));
            RuleFor(x => x.Key).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelKey, 50));
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelDisplayName));
            RuleFor(x => x.DisplayName).MaximumLength(100).WithMessage(string.Format(labelMaxLength, labelDisplayName, 100));
            RuleFor(x => x.Body).NotEmpty().WithMessage(string.Format(labelRequiredField, labelBody));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
