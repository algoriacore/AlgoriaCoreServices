using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates.Dto;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldCreateCommandValidator : AbstractValidator<TemplateFieldCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public TemplateFieldCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string labelGreaterThan = L("FieldGreaterThan");

            string labelTemplateSection = L("TemplateFields.TemplateField.TemplateSection");
            string labelName = L("TemplateFields.TemplateField.Name");
            string labelType = L("TemplateFields.TemplateField.Type");
            string labelControl = L("TemplateFields.TemplateField.Control");
            string labelSize = L("TemplateFields.TemplateField.Size");
            string labelKeyFilter = L("TemplateFields.TemplateField.KeyFilter");
            string labelOrder = L("TemplateFields.TemplateField.Order");

            RuleFor(x => x.TemplateSection).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTemplateSection));
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName));
            RuleFor(x => x.Name).MaximumLength(30).WithMessage(string.Format(labelMaxLength, labelName, 30));
            RuleFor(x => x.FieldType).NotEmpty().WithMessage(string.Format(labelRequiredField, labelType));
            RuleFor(x => x.FieldControl).NotEmpty().WithMessage(string.Format(labelRequiredField, labelControl));
            RuleFor(x => x.FieldSize).NotEmpty().When(x => x.FieldType == TemplateFieldType.Text).WithMessage(string.Format(labelRequiredField, labelSize));
            RuleFor(x => x.KeyFilter).NotEmpty().When(x => x.HasKeyFilter).WithMessage(string.Format(labelRequiredField, labelKeyFilter));
            RuleFor(x => x.KeyFilter).MaximumLength(500).WithMessage(string.Format(labelMaxLength, labelKeyFilter, 500));
            RuleFor(x => x.Order).NotEmpty().WithMessage(string.Format(labelRequiredField, labelOrder));
            RuleFor(x => x.Order).GreaterThan((short)0).When(x => x.Order.HasValue).WithMessage(string.Format(labelGreaterThan, labelOrder, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
