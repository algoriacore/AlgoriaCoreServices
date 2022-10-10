using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetForEditValidator : AbstractValidator<ProcessGetForEditQuery>
    {
        private readonly ICoreServices _coreServices;

        public ProcessGetForEditValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterOrEqualThan = L("FieldGreaterOrEqualThan");
            string labelGreaterOrEqualThanOrEmpty = L("FieldGreaterOrEqualThanOrEmpty");

            string labelTemplate = L("Template");
            string labelId = L("Id");

            RuleFor(x => x.Template).Must(x => x > 0).WithMessage(string.Format(labelGreaterOrEqualThan, labelTemplate, 0));
            RuleFor(x => x.Id).Must(x => !x.HasValue || x > 0).WithMessage(string.Format(labelGreaterOrEqualThanOrEmpty, labelId, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
