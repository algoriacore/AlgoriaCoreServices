using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageUpdateCommandValidator : AbstractValidator<LanguageUpdateCommand>
    {
        private readonly ICoreServices _coreServices;

        public LanguageUpdateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelId = L("Id");
            string labelName = L("Languages.Language.Name");
            string labelDisplayName = L("Languages.Language.DisplayName");

            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(labelRequiredField, labelId));
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
