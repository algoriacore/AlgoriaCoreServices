using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageCreateCommandValidator : AbstractValidator<LanguageCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public LanguageCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelName = L("Languages.Language.Name");
            string labelDisplayName = L("Languages.Language.DisplayName");

            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName))
                .MaximumLength(10).WithMessage(string.Format(labelMaxLength, labelName, 10));
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelDisplayName))
                .MaximumLength(100).WithMessage(string.Format(labelMaxLength, labelDisplayName, 100));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
