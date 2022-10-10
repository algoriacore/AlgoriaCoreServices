using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants._3Commands
{
    public class TenantDeleteCommandValidator : AbstractValidator<TenantDeleteCommand>
    {
        private readonly ICoreServices _coreServices;

        public TenantDeleteCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterThan = L("FieldGreaterThan");

            string labelId = L("Id");

            RuleFor(x => x.Id).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelId, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
