using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands
{
    public class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public RoleCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelName = L("Roles.NameForm");
            string labelDisplayName = L("Roles.DisplayNameForm");
            string labelIsActive = L("IsActive");

            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName));
            RuleFor(x => x.Name).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelName, 50));
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelDisplayName));
            RuleFor(x => x.DisplayName).MaximumLength(100).WithMessage(string.Format(labelMaxLength, labelDisplayName, 100));
            RuleFor(x => x.IsActive).NotEmpty().WithMessage(string.Format(labelRequiredField, labelIsActive));            
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
