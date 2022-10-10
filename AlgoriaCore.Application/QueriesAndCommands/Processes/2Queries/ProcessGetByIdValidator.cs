using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetByIdValidator : AbstractValidator<ProcessGetByIdQuery>
    {
        private readonly ICoreServices _coreServices;

        public ProcessGetByIdValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterThan = L("FieldGreaterThan");

            string labelTemplate = L("Templates.Template");
            string labelId = L("Id");

            RuleFor(x => x.Template).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelTemplate, 0));
            RuleFor(x => x.Id).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelId, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
