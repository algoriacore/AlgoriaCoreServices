using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.SecurityMembers
{
    public class ProcessSecurityMemberDeleteCommandValidator : AbstractValidator<ProcessSecurityMemberDeleteCommand>
    {
        private readonly ICoreServices _coreServices;

        public ProcessSecurityMemberDeleteCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelTemplate = L("ProcessSecurity.Template");
            string labelId = L("Id");
            string labelType = L("ProcessSecurity.Type");
            string labelLevel = L("ProcessSecurity.Level");

            RuleFor(x => x.Template).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTemplate));
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
