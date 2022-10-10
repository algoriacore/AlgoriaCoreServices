using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpGetByKeyForCurrentUserValidator : AbstractValidator<HelpGetByKeyForCurrentUserQuery>
    {
        private readonly ICoreServices _coreServices;

        public HelpGetByKeyForCurrentUserValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelKey = L("Helps.Help.Key");

            RuleFor(x => x.Key).NotEmpty().WithMessage(string.Format(labelRequiredField, labelKey));
            RuleFor(x => x.Key).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelKey, 50));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
