﻿using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Extensions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
    {
        private readonly ICoreServices _coreServices;

        public UserUpdateCommandValidator(ICoreServices coreServices, ILogger<UserUpdateCommandValidator> logger)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string labelInvalidMailAddress = L("FieldInvalidEmailAddress");

            string labelId = L("Id");
            string labelName = L("Users.NameForm");
            string labelLastName = L("Users.LastNameForm");
            string labelSecondLastName = L("Users.SecondLastNameForm");
            string labelEmail = L("Users.EmailAddressForm");
            string labelPhoneNumber = L("Users.PhoneNumberForm");
            string labelUserName = L("Users.UserNameForm");

            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(labelRequiredField, labelId));
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName));
            RuleFor(x => x.Name).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelName, 50));
            RuleFor(x => x.LastName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelLastName));
            RuleFor(x => x.LastName).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelLastName, 50));
            RuleFor(x => x.SecondLastName).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelSecondLastName, 50));
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage(string.Format(labelRequiredField, labelEmail));

            Unless(x => x.EmailAddress.IsNullOrWhiteSpace(), () => {
                RuleFor(x => x.EmailAddress).MaximumLength(250).WithMessage(string.Format(labelMaxLength, labelEmail, 250));
                RuleFor(x => x.EmailAddress).EmailAddress().WithMessage(string.Format(labelInvalidMailAddress, labelEmail));
            });

            RuleFor(x => x.PhoneNumber).MaximumLength(20).WithMessage(string.Format(labelMaxLength, labelPhoneNumber, 20));
            RuleFor(x => x.UserName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelUserName));
            RuleFor(x => x.UserName).MaximumLength(32).WithMessage(string.Format(labelMaxLength, labelUserName, 32));
            When(x => !x.Password.IsNullOrEmpty(), () =>
              {
                  RuleFor(x => x.PasswordRepeat).Equal(x => x.Password).WithMessage(L("Users.PasswordsDontMatchForm"));
              });
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
