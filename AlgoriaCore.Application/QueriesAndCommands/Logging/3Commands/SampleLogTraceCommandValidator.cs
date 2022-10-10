using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Logging._3Commands
{
    public class SampleLogTraceCommandValidator : AbstractValidator<SampleLogTraceCommand>
    {
        private readonly ICoreServices _coreServices;

        public SampleLogTraceCommandValidator(ICoreServices coreServices)
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
