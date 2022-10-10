using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Logging._3Commands
{
    public class SampleLogErrorCommandValidator : AbstractValidator<SampleLogErrorCommand>
    {
        private readonly ICoreServices _coreServices;

        public SampleLogErrorCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelMessage = L("Examples.Log.Message");

            RuleFor(x => x.Message).NotEmpty().WithMessage(string.Format(labelRequiredField, labelMessage));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
