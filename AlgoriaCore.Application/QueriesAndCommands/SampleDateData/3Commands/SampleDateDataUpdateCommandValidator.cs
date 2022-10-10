using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataUpdateCommandValidator : AbstractValidator<SampleDateDataUpdateCommand>
    {
        private readonly ICoreServices _coreServices;

        public SampleDateDataUpdateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelId = L("Id");
            string labelName = L("Examples.DateTimes.Name");
            string labelDateTime = L("Examples.DateTimes.DateTime");
            string labelDate = L("Examples.DateTimes.Date");
            string labelTime = L("Examples.DateTimes.Time");

            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(labelRequiredField, labelId));
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName))
                .MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelName, 50));
            RuleFor(x => x.DateTimeData).NotEmpty().WithMessage(string.Format(labelRequiredField, labelDateTime));
            RuleFor(x => x.DateData).NotEmpty().WithMessage(string.Format(labelRequiredField, labelDate));
            RuleFor(x => x.TimeData).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTime));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
