using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Extensions;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._3Commands
{
    public class UserChangePasswordValidator : AbstractValidator<UserChangePasswordCommand>
    {
        private readonly ICoreServices _coreServices;

        public UserChangePasswordValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelPassword = L("ChangePassword.Password");
            string labelNewPassword = L("ChangePassword.NewPassword");
            string labelPasswordsDontMatch = L("Register.User.PasswordsDontMatch");
            
            //Todas las reglas de validación del Login
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage(string.Format(labelRequiredField, labelPassword));
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(string.Format(labelRequiredField, labelNewPassword));

            When(m => !m.NewPassword.IsNullOrEmpty(), () =>
               {
                   RuleFor(x => x.ConfirmPassword).Equal(m => m.NewPassword).WithMessage(labelPasswordsDontMatch);
               });
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
