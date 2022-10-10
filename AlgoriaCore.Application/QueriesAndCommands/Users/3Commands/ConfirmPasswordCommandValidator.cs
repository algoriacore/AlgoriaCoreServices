using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserChangePasswordValidator : AbstractValidator<ConfirmPasswordCommandReset>
    {
        private readonly ICoreServices _coreServices;

        public UserChangePasswordValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelConfirmationCode = L("Register.Tenant.ConfirmationCode");
            string labelPassword = L("Register.User.Password");
            string labelPasswordsDontMatch = L("Register.User.PasswordsDontMatch");
            
            //Todas las reglas de validación del Login
            RuleFor(x => x.ConfirmationCode).NotEmpty().WithMessage(string.Format(labelRequiredField, labelConfirmationCode));
            RuleFor(x => x.Password).NotEmpty().WithMessage(string.Format(labelRequiredField, labelPassword));
            RuleFor(x => x.ConfirmPassword).Equal(m => m.Password).WithMessage(labelPasswordsDontMatch);
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
