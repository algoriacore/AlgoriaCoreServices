using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetCreateCommandValidator : AbstractValidator<ToDoTimeSheetCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public ToDoTimeSheetCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterThan = L("FieldGreaterThan");
            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelActivity = L("Processes.Process.ToDoTimeSheets.ToDoTimeSheet.Activity");
            string labelCreationDate = L("Processes.Process.ToDoTimeSheets.ToDoTimeSheet.CreationDate");
            string labelComments = L("Processes.Process.ToDoTimeSheets.ToDoTimeSheet.Comments");

            RuleFor(x => x.Activity).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelActivity, 0));
            RuleFor(x => x.CreationDate).NotEmpty().WithMessage(string.Format(labelRequiredField, labelCreationDate));
            RuleFor(x => x.Comments).NotEmpty().WithMessage(string.Format(labelRequiredField, labelComments));
            RuleFor(x => x.Comments).MaximumLength(250).WithMessage(string.Format(labelMaxLength, labelComments, 250));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
