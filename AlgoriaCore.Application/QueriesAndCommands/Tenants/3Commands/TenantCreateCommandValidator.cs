using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Extensions;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands
{
    public class TenantCreateCommandValidator : AbstractValidator<TenantCreateCommand>
	{
		private readonly ICoreServices _coreServices;

		public TenantCreateCommandValidator(ICoreServices coreServices)
		{
			_coreServices = coreServices;

			string labelRequiredField = L("RequiredField");
			string labelMinLength = L("FieldMinLength");
			string labelMaxLength = L("FieldMaxLength");
			string labelInvalidMailAddress = L("FieldInvalidEmailAddress");
			string labelNoSpaces = L("FieldNoSpaces");

			string labelInstance = L("Register.User.Instance");
			string labelInstanceName = L("Register.User.InstanceName");
			string labelName = L("Register.User.Name");
			string labelLastName = L("Register.User.LastName");
			string labelPassword = L("Register.User.Password");
			string labelPasswordConfirm = L("Register.User.PasswordConfirm");
			string labelPasswordsDontMatch = L("Register.User.PasswordsDontMatch");
			string labelEmail = L("Register.User.Email");

			RuleFor(x => x.TenancyName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelInstance));

			Unless(x => x.TenancyName.IsNullOrWhiteSpace(), () =>
			{
				RuleFor(x => x.TenancyName).MinimumLength(2).WithMessage(string.Format(labelMinLength, labelInstance, 2))
				.MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelInstance, 50))
				.Matches("^[^\\s]*$").WithMessage(string.Format(labelNoSpaces, labelInstance));
			});

			RuleFor(x => x.TenantName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelInstanceName))
				.MaximumLength(150).WithMessage(string.Format(labelMaxLength, labelInstanceName, 150));
			RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName))
				.MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelName, 50));
			RuleFor(x => x.LastName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelLastName))
				.MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelLastName, 50));
			RuleFor(x => x.SecondLastName).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelLastName, 50));
			RuleFor(x => x.Password).NotEmpty().WithMessage(string.Format(labelRequiredField, labelPassword))
				.MaximumLength(200).WithMessage(string.Format(labelMaxLength, labelPassword, 200));
			RuleFor(x => x.PasswordConfirm).MaximumLength(200).WithMessage(string.Format(labelMaxLength, labelPasswordConfirm, 200))
				.Equal(x => x.Password).WithMessage(labelPasswordsDontMatch);
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
