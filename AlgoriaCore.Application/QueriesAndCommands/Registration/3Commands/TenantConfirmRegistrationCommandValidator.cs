using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands
{
    public class TenantConfirmRegistrationCommandValidator : AbstractValidator<TenantConfirmRegistrationCommand>
    {
        private readonly ICoreServices _coreServices;

        public TenantConfirmRegistrationCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelConfirmationCode = L("Register.Tenant.ConfirmationCode");

            RuleFor(m => m.Code).NotEmpty().WithMessage(string.Format(labelRequiredField, labelConfirmationCode));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
