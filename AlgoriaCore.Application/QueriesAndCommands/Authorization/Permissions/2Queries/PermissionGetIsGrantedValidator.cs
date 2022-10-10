using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Authorization.Permissions
{
    public class PermissionGetIsGrantedValidator : AbstractValidator<PermissionGetIsGrantedQuery>
    {
        private readonly ICoreServices _coreServices;

        public PermissionGetIsGrantedValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelTemplate = L("Processes.Process.Template");

            RuleFor(x => x.Template).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTemplate)).When(p =>p.IsTemplateProcess);
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
