using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants
{
    public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
    {
        private readonly ICoreServices _coreServices;

        public UpdateTenantCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMinLength = L("FieldMinLength");
            string labelMaxLength = L("FieldMaxLength");

            string labelId = L("Id");
            string labelInstance = L("Register.User.Instance");
            string labelName = L("Register.User.Name");

            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(labelRequiredField, labelId));
            RuleFor(x => x.TenancyName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelInstance))
                .MinimumLength(2).WithMessage(string.Format(labelMinLength, labelInstance, 2))
                .MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelInstance, 50));
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName))
                .MaximumLength(150).WithMessage(string.Format(labelMaxLength, labelName, 150));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
