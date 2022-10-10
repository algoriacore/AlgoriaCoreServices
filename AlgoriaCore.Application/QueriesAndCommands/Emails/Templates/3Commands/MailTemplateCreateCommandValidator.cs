using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Extensions;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._3Commands
{
    public class MailTemplateCreateCommandValidator : AbstractValidator<MailTemplateCreateCommand>
    {
        private readonly ICoreServices _coreServices;

        public MailTemplateCreateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string labelMailRegEx = L("ValidMailRegEx");

            string labelMailGroup = L("EmailTemplates.MailGroupForm");
            string labelMailKey = L("EmailTemplates.MailKeyForm");
            string labelDisplayName = L("EmailTemplates.DisplayNameForm");
            string labelSendTo = L("EmailTemplates.SendToForm");
            string labelCopyTo = L("EmailTemplates.CopyToForm");
            string labelBlindCopyTo = L("EmailTemplates.BlindCopyToForm");
            string labelSubject = L("EmailTemplates.SubjectForm");

            RuleFor(x => x.MailGroup).NotEmpty().WithMessage(string.Format(labelRequiredField, labelMailGroup));
            RuleFor(x => x.MailKey).NotEmpty().WithMessage(string.Format(labelRequiredField, labelMailKey));
            RuleFor(x => x.MailKey).MaximumLength(50).WithMessage(string.Format(labelMaxLength, labelMailKey, 50));
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelDisplayName));
            RuleFor(x => x.DisplayName).MaximumLength(100).WithMessage(string.Format(labelMaxLength, labelDisplayName, 100));

            When(x => !x.SendTo.IsNullOrWhiteSpace(), () =>
            {
                RuleFor(x => x.SendTo)
                .MaximumLength(1000).WithMessage(string.Format(labelMaxLength, labelSendTo, 1000))
                .Matches(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").WithMessage(string.Format(labelMailRegEx, labelSendTo, 1000));

            });

            When(x => !x.CopyTo.IsNullOrWhiteSpace(), () =>
            {
                RuleFor(x => x.CopyTo)
                .MaximumLength(1000).WithMessage(string.Format(labelMaxLength, labelCopyTo, 1000))
                .Matches(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").WithMessage(string.Format(labelMailRegEx, labelCopyTo, 1000));

            });

            When(x => !x.BlindCopyTo.IsNullOrWhiteSpace(), () =>
            {
                RuleFor(x => x.BlindCopyTo)
                .MaximumLength(1000).WithMessage(string.Format(labelMaxLength, labelBlindCopyTo, 1000))
                .Matches(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").WithMessage(string.Format(labelMailRegEx, labelBlindCopyTo, 1000));

            });
            RuleFor(x => x.Subject).MaximumLength(250).WithMessage(string.Format(labelMaxLength, labelSubject, 250));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
