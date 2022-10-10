using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateCreateCommandValidator : AbstractValidator<TemplateCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public TemplateCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelNameSingular = L("Templates.Template.NameSingular");
            string labelNamePlural = L("Templates.Template.NamePlural");
            string labelRGBColor = L("Templates.Template.RGBColor");

            RuleFor(x => x.NameSingular).NotEmpty().WithMessage(string.Format(labelRequiredField, labelNameSingular));
            RuleFor(x => x.NameSingular).MaximumLength(20).WithMessage(string.Format(labelMaxLength, labelNameSingular, 20));
            RuleFor(x => x.NamePlural).NotEmpty().WithMessage(string.Format(labelRequiredField, labelNamePlural));
            RuleFor(x => x.NamePlural).MaximumLength(22).WithMessage(string.Format(labelMaxLength, labelNamePlural, 22));
            RuleFor(x => x.Description).NotEmpty().WithMessage(string.Format(labelRequiredField, labelNamePlural));
            RuleFor(x => x.Description).MaximumLength(1000).WithMessage(string.Format(labelMaxLength, labelNamePlural, 1000));
            RuleFor(x => x.RGBColor).NotEmpty().WithMessage(string.Format(labelRequiredField, labelRGBColor));
            RuleFor(x => x.RGBColor).MaximumLength(6).WithMessage(string.Format(labelMaxLength, labelRGBColor, 6));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
