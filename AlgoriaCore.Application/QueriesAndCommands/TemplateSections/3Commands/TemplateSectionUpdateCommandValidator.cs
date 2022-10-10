using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionUpdateCommandValidator : AbstractValidator<TemplateSectionUpdateCommand>
    {
        private readonly ICoreServices _coreServices;

        public TemplateSectionUpdateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string labelGreaterThan = L("FieldGreaterThan");

            string labelId = L("Id");
            string labelTemplate = L("TemplateSections.TemplateSection.Template");
            string labelName = L("TemplateSections.TemplateSection.Name");
            string labelOrder = L("TemplateSections.TemplateSection.Order");
            string labelIconAF = L("TemplateSections.TemplateSection.IconAF");

            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(labelRequiredField, labelId));
            RuleFor(x => x.Template).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTemplate));
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName));
            RuleFor(x => x.Name).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelName, 50));
            RuleFor(x => x.Order).NotEmpty().WithMessage(string.Format(labelRequiredField, labelOrder));
            RuleFor(x => x.Order).GreaterThan((short)0).When(x => x.Order.HasValue).WithMessage(string.Format(labelGreaterThan, labelOrder, 0));
            RuleFor(x => x.IconAF).MaximumLength(20).WithMessage(string.Format(labelMaxLength, labelIconAF, 20));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
