using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusCreateCommandValidator : AbstractValidator<TemplateToDoStatusCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public TemplateToDoStatusCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string labelGreaterThan = L("FieldGreaterThan");

            string labelTemplate = L("TemplateToDoStatus.TemplateToDoStatus.Template");
            string labelName = L("TemplateToDoStatus.TemplateToDoStatus.Name");
            string labelType = L("TemplateToDoStatus.TemplateToDoStatus.Type");

            RuleFor(x => x.Template).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTemplate));
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName));
            RuleFor(x => x.Name).MaximumLength(30).WithMessage(string.Format(labelMaxLength, labelName, 30));
            RuleFor(x => x.Type).NotEmpty().WithMessage(string.Format(labelRequiredField, labelType));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
