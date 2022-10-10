using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitUpdateCommandValidator : AbstractValidator<OrgUnitUpdateCommand>
    {
        private readonly ICoreServices _coreServices;

        public OrgUnitUpdateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterThan = L("FieldGreaterThan");
            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelId = L("Id");
            string labelName = L("OrgUnits.OrgUnit.Name");

            RuleFor(x => x.Id).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelId, 0));
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName));
            RuleFor(x => x.Name).MaximumLength(100).WithMessage(string.Format(labelMaxLength, labelName, 100));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
