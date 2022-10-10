using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnitUsers.OrgUnitUsers
{
    public class OrgUnitUserCreateCommandValidator : AbstractValidator<OrgUnitUserCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public OrgUnitUserCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterThan = L("FieldGreaterThan");

            string labelUser = L("OrgUnitUsers.OrgUnitUser.User");
            string labelOrgUnit = L("OrgUnitUsers.OrgUnitUser.OrgUnit");

            RuleFor(x => x.OrgUnit).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelOrgUnit, 0));
            RuleFor(x => x.User).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelUser, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
