using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates.Dto;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers
{
    public class TemplateSecurityMemberCreateCommandValidator : AbstractValidator<TemplateSecurityMemberCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public TemplateSecurityMemberCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            
            string labelTemplate = L("TemplateSecurity.Template");
            string labelType = L("TemplateSecurity.Type");
            string labelUser = L("TemplateSecurity.Type.User");
            string labelOrgUnit = L("TemplateSecurity.Type.OrgUnit");
            string labelLevel = L("TemplateSecurity.Level");

            RuleFor(x => x.Template).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTemplate));
            RuleFor(x => x.Type).NotEmpty().WithMessage(string.Format(labelRequiredField, labelType));

            When(x => x.Type == SecurityMemberType.User, () =>
            {
                RuleFor(x => x.Member).NotEmpty().WithMessage(string.Format(labelRequiredField, labelUser));
            });

            When(x => x.Type == SecurityMemberType.OrgUnit, () =>
            {
                RuleFor(x => x.Member).NotEmpty().WithMessage(string.Format(labelRequiredField, labelOrgUnit));
            });

            RuleFor(x => x.Level).NotEmpty().WithMessage(string.Format(labelRequiredField, labelLevel));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
