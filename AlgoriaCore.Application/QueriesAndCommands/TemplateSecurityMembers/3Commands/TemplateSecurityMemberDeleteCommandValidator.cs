using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers
{
    public class TemplateSecurityMemberDeleteCommandValidator : AbstractValidator<TemplateSecurityMemberDeleteCommand>
    {
        private readonly ICoreServices _coreServices;

        public TemplateSecurityMemberDeleteCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelId = L("Id");
            string labelType = L("TemplateSecurity.Type");
            string labelLevel = L("TemplateSecurity.Level");

            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(labelRequiredField, labelId));
            RuleFor(x => x.Type).NotEmpty().WithMessage(string.Format(labelRequiredField, labelType));
            RuleFor(x => x.Level).NotEmpty().WithMessage(string.Format(labelRequiredField, labelLevel));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
