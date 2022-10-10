using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Extensions;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands
{
    public class TenantCreateRegistrationCommandValidator : AbstractValidator<TenantCreateRegistrationCommand>
	{
		private readonly ICoreServices _coreServices;

		public TenantCreateRegistrationCommandValidator(ICoreServices coreServices)
		{
			_coreServices = coreServices;

			string labelRequiredField = L("RequiredField");
			string labelMinLength = L("FieldMinLength");
			string labelNoSpaces = L("FieldNoSpaces");

			string labelInstance = L("Register.User.Instance");
			string labelInstanceName = L("Register.User.InstanceName");
			string labelName = L("Register.User.Name");
			string labelLastName = L("Register.User.LastName");
			string labelPassword = L("Register.User.Password");
			string labelPasswordsDontMatch = L("Register.User.PasswordsDontMatch");
			string labelEmail = L("Register.User.Email");
			string labelMaxLength = L("FieldMaxLength");
			string labelInvalidMailAddress = L("FieldInvalidEmailAddress");

			RuleFor(x => x.TenancyName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelInstance));
			Unless(x => x.TenancyName.IsNullOrWhiteSpace(), () =>
			{
				RuleFor(x => x.TenancyName).MinimumLength(2).WithMessage(string.Format(labelMinLength, labelInstance, 2))
				.MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelInstance, 50))
				.Matches("^[^\\s]*$").WithMessage(string.Format(labelNoSpaces, labelInstance));
			});

			RuleFor(x => x.TenantName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelInstanceName));
			RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName));
			RuleFor(x => x.LastName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelLastName));
			RuleFor(x => x.Password).NotEmpty().WithMessage(string.Format(labelRequiredField, labelPassword));
			RuleFor(x => x.PasswordConfirm).Equal(x => x.Password).WithMessage(labelPasswordsDontMatch);
			RuleFor(x => x.EmailAddress).NotEmpty().WithMessage(string.Format(labelRequiredField, labelEmail));

			RuleFor(x => x.EmailAddress).NotEmpty().WithMessage(string.Format(labelRequiredField, labelEmail));

			Unless(x => x.EmailAddress.IsNullOrWhiteSpace(), () =>
			{
				RuleFor(x => x.EmailAddress).MaximumLength(250).WithMessage(string.Format(labelMaxLength, labelEmail, 250));
				RuleFor(x => x.EmailAddress).EmailAddress().WithMessage(string.Format(labelInvalidMailAddress, labelEmail));
			});
		}

		private string L(string name)
		{
			return _coreServices.AppLocalizationProvider.L(name);
		}
	}
}
