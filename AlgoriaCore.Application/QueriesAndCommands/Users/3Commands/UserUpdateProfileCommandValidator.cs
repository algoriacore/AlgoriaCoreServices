using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Extensions;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserUpdateProfileCommandValidator : AbstractValidator<UserUpdateProfileCommand>
    {
        private readonly ICoreServices _coreServices;

        public UserUpdateProfileCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string labelInvalidMailAddress = L("FieldInvalidEmailAddress");

            string labelName = L("Users.NameForm");
            string labelLastName = L("Users.LastNameForm");
            string labelSecondLastName = L("Users.SecondLastNameForm");
            string labelEmail = L("Users.EmailAddressForm");
            string labelPhoneNumber = L("Users.PhoneNumberForm");
            string labelNewPassword = L("Users.NewPasswordForm");
            string labelNewPasswordsDontMatch = L("Users.PasswordsDontMatchForm");

            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName));
            RuleFor(x => x.Name).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelName, 50));
            RuleFor(x => x.LastName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelLastName));
            RuleFor(x => x.LastName).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelLastName, 50));
            RuleFor(x => x.SecondLastName).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelSecondLastName, 50));
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage(string.Format(labelRequiredField, labelEmail));
            RuleFor(x => x.EmailAddress).MaximumLength(250).WithMessage(string.Format(labelMaxLength, labelEmail, 250));
            RuleFor(x => x.EmailAddress).EmailAddress().WithMessage(string.Format(labelInvalidMailAddress, labelEmail));
            RuleFor(x => x.PhoneNumber).MaximumLength(20).WithMessage(string.Format(labelMaxLength, labelPhoneNumber, 20));

            When(m => !m.CurrentPassword.IsNullOrEmpty(), () =>
               {
                   RuleFor(x => x.NewPassword).NotEmpty().WithMessage(string.Format(labelRequiredField, labelNewPassword));
               });

            When(m => !m.CurrentPassword.IsNullOrEmpty() && !m.NewPassword.IsNullOrEmpty(), () =>
            {
                RuleFor(x => x.NewPassword).Equal(x => x.NewPasswordConfirm).WithMessage(labelNewPasswordsDontMatch);
            });
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
