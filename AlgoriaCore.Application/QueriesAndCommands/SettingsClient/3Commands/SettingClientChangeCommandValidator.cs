using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.SettingsClient
{
    public class SettingClientChangeCommandValidator : AbstractValidator<SettingClientChangeCommand>
    {
        private readonly ICoreServices _coreServices;

        public SettingClientChangeCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelFieldClientType = L("SettingClient.ClientType");
            string labelFieldName = L("SettingClient.Name");

            RuleFor(x => x.ClientType).NotEmpty().WithMessage(string.Format(labelRequiredField, labelFieldClientType));
            RuleFor(x => x.ClientType).MaximumLength(256).WithMessage(string.Format(labelMaxLength, labelFieldClientType, 256));
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelFieldClientType));
            RuleFor(x => x.Name).MaximumLength(256).WithMessage(string.Format(labelMaxLength, labelFieldName, 256));
            RuleFor(x => x.Value).NotEmpty().WithMessage(string.Format(labelRequiredField, labelFieldClientType));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
