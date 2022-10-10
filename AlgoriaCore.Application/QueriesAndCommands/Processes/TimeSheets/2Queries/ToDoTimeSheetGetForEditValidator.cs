using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetGetForEditValidator : AbstractValidator<ToDoTimeSheetGetForEditQuery>
    {
        private readonly ICoreServices _coreServices;

        public ToDoTimeSheetGetForEditValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterOrEqualThanOrEmpty = L("FieldGreaterOrEqualThanOrEmpty");
            string labelGreaterThan = L("FieldGreaterThan");

            string labelTemplate = L("Processes.Process.Template");
            string labelId = L("Id");

            RuleFor(x => x.Id).Must(x => !x.HasValue || x > 0).WithMessage(string.Format(labelGreaterOrEqualThanOrEmpty, labelId, 0));
            RuleFor(x => x.Template).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelTemplate, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
