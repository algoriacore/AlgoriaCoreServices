using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextUpdateCommandValidator : AbstractValidator<LanguageTextUpdateCommand>
    {
        private readonly ICoreServices _coreServices;

        public LanguageTextUpdateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelLanguage = L("Languages.Texts.Text.Language");
            string labelKey = L("Languages.Texts.Text.Key");
            string labelValue = L("Languages.Texts.Text.Value");

            RuleFor(x => x.LanguageId).NotEmpty().WithMessage(string.Format(labelRequiredField, labelLanguage));
            RuleFor(x => x.Key).NotEmpty().WithMessage(string.Format(labelRequiredField, labelKey))
                .MaximumLength(100).WithMessage(string.Format(labelMaxLength, labelKey, 100));
            RuleFor(x => x.Value).NotEmpty().WithMessage(string.Format(labelRequiredField, labelValue));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
