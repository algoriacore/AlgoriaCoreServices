using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._3Commands
{
    public class MailTemplateSendTestCommandValidator : AbstractValidator<MailTemplateSendTestCommand>
    {
        private readonly ICoreServices _coreServices;

        public MailTemplateSendTestCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");

            string labelMailGroup = L("EmailTemplates.MailGroupForm");
            string labelEmail = L("EmailTemplates.Test.EmailForm");
            string labelSubject = L("EmailTemplates.SubjectForm");

            RuleFor(x => x.MailGroup).NotEmpty().WithMessage(string.Format(labelRequiredField, labelMailGroup));
            RuleFor(x => x.Email).NotEmpty().WithMessage(string.Format(labelRequiredField, labelEmail));
            RuleFor(x => x.Subject).MaximumLength(250).WithMessage(string.Format(labelMaxLength, labelSubject, 250));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
