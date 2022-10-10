using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetForReadValidator : AbstractValidator<ProcessGetForReadQuery>
    {
        private readonly ICoreServices _coreServices;

        public ProcessGetForReadValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterOrEqualThan = L("FieldGreaterOrEqualThan");

            string labelTemplate = L("Template");
            string labelId = L("Id");

            RuleFor(x => x.Template).Must(x => x > 0).WithMessage(string.Format(labelGreaterOrEqualThan, labelTemplate, 0));
            RuleFor(x => x.Id).Must(x => x > 0).WithMessage(string.Format(labelGreaterOrEqualThan, labelId, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
