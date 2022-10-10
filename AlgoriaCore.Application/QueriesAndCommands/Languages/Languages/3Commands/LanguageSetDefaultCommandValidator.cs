using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageSetDefaultCommandValidator : AbstractValidator<LanguageSetDefaultCommand>
    {
        private readonly ICoreServices _coreServices;

        public LanguageSetDefaultCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelId = L("Id");

            RuleFor(x => x.Language).NotEmpty().WithMessage(string.Format(labelRequiredField, labelId));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
