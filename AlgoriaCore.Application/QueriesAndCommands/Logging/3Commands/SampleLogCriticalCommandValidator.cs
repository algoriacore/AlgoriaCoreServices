using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Extensions;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Logging._3Commands
{
    public class SampleLogCriticalCommandValidator : AbstractValidator<SampleLogCriticalCommand>
    {
        private readonly ICoreServices _coreServices;

        public SampleLogCriticalCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelInvalidMailAddress = L("FieldInvalidEmailAddress");

            string labelMessage = L("Examples.Log.Message");
            string labelEmail = L("Examples.Log.CriticalEmail");

            RuleFor(x => x.Message).NotEmpty().WithMessage(string.Format(labelRequiredField, labelMessage));
            RuleFor(x => x.Email).NotEmpty().WithMessage(string.Format(labelRequiredField, labelEmail));

            Unless(x => x.Email.IsNullOrWhiteSpace(), () => {
                RuleFor(x => x.Email).EmailAddress().WithMessage(string.Format(labelInvalidMailAddress, labelEmail));
            });
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
