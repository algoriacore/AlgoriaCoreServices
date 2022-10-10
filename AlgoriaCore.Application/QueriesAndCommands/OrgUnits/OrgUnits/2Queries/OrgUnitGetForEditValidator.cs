using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetForEditValidator : AbstractValidator<OrgUnitGetForEditQuery>
    {
        private readonly ICoreServices _coreServices;

        public OrgUnitGetForEditValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterOrEqualThanOrEmpty = L("FieldGreaterOrEqualThanOrEmpty");

            string labelId = L("Id");

            RuleFor(x => x.Id).Must(x => !x.HasValue || x > 0).WithMessage(string.Format(labelGreaterOrEqualThanOrEmpty, labelId, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
