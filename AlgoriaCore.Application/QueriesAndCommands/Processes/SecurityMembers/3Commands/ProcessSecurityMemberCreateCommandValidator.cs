using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates.Dto;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.SecurityMembers
{
    public class ProcessSecurityMemberCreateCommandValidator : AbstractValidator<ProcessSecurityMemberCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public ProcessSecurityMemberCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            
            string labelTemplate = L("ProcessSecurity.Template");
            string labelParent = L("ProcessSecurity.Parent");
            string labelType = L("ProcessSecurity.Type");
            string labelUser = L("ProcessSecurity.Type.User");
            string labelOrgUnit = L("ProcessSecurity.Type.OrgUnit");
            string labelLevel = L("ProcessSecurity.Level");

            RuleFor(x => x.Template).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTemplate));
            RuleFor(x => x.Parent).NotEmpty().WithMessage(string.Format(labelRequiredField, labelParent));
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
