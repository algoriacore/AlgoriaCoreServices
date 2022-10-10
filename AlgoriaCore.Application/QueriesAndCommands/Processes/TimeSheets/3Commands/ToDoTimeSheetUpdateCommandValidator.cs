using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetUpdateCommandValidator : AbstractValidator<ToDoTimeSheetUpdateCommand>
    {
        private readonly ICoreServices _coreServices;

        public ToDoTimeSheetUpdateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterThan = L("FieldGreaterThan");
            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelId = L("Id");
            string labelCreationDate = L("Processes.Process.ToDoTimeSheets.ToDoTimeSheet.CreationDate");
            string labelComments = L("Processes.Process.ToDoTimeSheets.ToDoTimeSheet.Comments");

            RuleFor(x => x.Id).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelId, 0));
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
