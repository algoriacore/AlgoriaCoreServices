using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataConvertCommandValidator : AbstractValidator<SampleDateDataConvertCommand>
    {
        private readonly ICoreServices _coreServices;

        public SampleDateDataConvertCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelTimeZoneFrom = L("Examples.DateTimes.TimeZoneFrom");
            string labelTimeZoneTo = L("Examples.DateTimes.TimeZoneto");
            string labelDateTimeDataToConvert = L("Examples.DateTimes.DateTimeDataToConvert");

            RuleFor(x => x.TimeZoneFrom).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTimeZoneFrom));
            RuleFor(x => x.TimeZoneTo).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTimeZoneTo));
            RuleFor(x => x.DateTimeDataToConvert).NotEmpty().WithMessage(string.Format(labelRequiredField, labelDateTimeDataToConvert));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
