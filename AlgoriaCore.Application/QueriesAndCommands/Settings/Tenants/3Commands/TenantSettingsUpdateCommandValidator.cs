using AlgoriaCore.Application.Configuration;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Extensions;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Tenant
{
    public class TenantSettingsUpdateCommandValidator : AbstractValidator<TenantSettingsUpdateCommand>
    {
        private readonly ICoreServices _coreServices;

        public TenantSettingsUpdateCommandValidator(ICoreServices coreServices, IOptions<EmailOptions> emailOptions)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string labelGreaterThan = L("FieldGreaterThan");
            string labelGreaterOrEqualThan = L("FieldGreaterOrEqualThan");
            string labelGreaterOrEqualThanOrEmpty = L("FieldGreaterOrEqualThanOrEmpty");
            string labelInvalidMailAddress = L("FieldInvalidEmailAddress");

            string labelFieldSenderDefault = L("Tenant.Settings.Mail.SenderDefault");
            string labelFieldHost = L("Tenant.Settings.Mail.Host");
            string labelFieldUserName = L("Tenant.Settings.Mail.UserName");
            string labelFieldUserPassword = L("Tenant.Settings.Mail.UserPassword");

            string labelFieldGrpcMail = L("Host.Settings.Mail.GrpcMail");
            string labelFieldGrpcMailTenancyName = L("Tenant.Settings.Mail.GrpcMail.TenancyName");
            string labelFieldGrpcMailUserName = L("Tenant.Settings.Mail.GrpcMail.UserName");
            string labelFieldGrpcMailPassword = L("Tenant.Settings.Mail.GrpcMail.Password");

            RuleFor(x => x.PasswordComplexity.MinimumLength).Must((x, value) =>
            {
                return x.PasswordUseDefaultConfiguration || value > 0;
            }).WithMessage(string.Format(labelGreaterThan, L("Tenant.Settings.Security.PasswordMinimumLength"), 0));

            RuleFor(x => x.PasswordComplexity.MaximumLength).Must((x, value) =>
            {
                return x.PasswordUseDefaultConfiguration || value > 0;
            }).WithMessage(string.Format(labelGreaterThan, L("Tenant.Settings.Security.PasswordMaximumLength"), 0));

            RuleFor(x => x.PasswordValidDays).GreaterThanOrEqualTo(0).WithMessage(string.Format(labelGreaterOrEqualThan, L("Tenant.Settings.Security.PasswordValidDays"), 0));

            RuleFor(x => x.FailedAttemptsToBlockUser).Must((x, value) =>
            {
                return !x.EnableUserBlocking || value > 0;
            }).WithMessage(string.Format(labelGreaterThan, L("Tenant.Settings.Security.FailedAttemptsToBlockUser"), 0));

            RuleFor(x => x.UserBlockingDuration).Must((x, value) =>
            {
                return !x.EnableUserBlocking || value > 0;
            }).WithMessage(string.Format(labelGreaterThan, L("Tenant.Settings.Security.UserBlockingDuration"), 0));

            RuleFor(x => x.MailSMTPSenderDefault).MaximumLength(256).WithMessage(string.Format(labelMaxLength, labelFieldSenderDefault, 256))
            .EmailAddress().WithMessage(string.Format(labelInvalidMailAddress, labelFieldSenderDefault));

            RuleFor(x => x.MailSMTPSenderDefaultDisplayName).MaximumLength(128).WithMessage(string.Format(labelMaxLength, L("Tenant.Settings.Mail.SenderDefaultDisplayName"), 128));
            RuleFor(x => x.MailSMTPHost).MaximumLength(64).WithMessage(string.Format(labelMaxLength, labelFieldHost, 64));
            RuleFor(x => x.MailSMTPPort).GreaterThanOrEqualTo(0).WithMessage(string.Format(labelGreaterOrEqualThanOrEmpty, labelFieldHost, 0));

            RuleFor(x => x.MailDomainName).MaximumLength(128).When(x => !x.MailDomainName.IsNullOrWhiteSpace()).WithMessage(string.Format(labelMaxLength, L("Tenant.Settings.Mail.DomainName"), 128));
            RuleFor(x => x.MailUserName).NotEmpty().When(x => !x.MailUseDefaultCredentials).WithMessage(string.Format(labelRequiredField, labelFieldUserName));
            RuleFor(x => x.MailUserName).MaximumLength(128).When(x => !x.MailUserName.IsNullOrWhiteSpace()).WithMessage(string.Format(labelMaxLength, labelFieldUserName, 128));
            RuleFor(x => x.MailUserPassword).NotEmpty().When(x => !x.MailUseDefaultCredentials).WithMessage(string.Format(labelRequiredField, labelFieldUserPassword));
            RuleFor(x => x.MailUserPassword).MaximumLength(128).When(x => !x.MailUserPassword.IsNullOrWhiteSpace()).WithMessage(string.Format(labelMaxLength, labelFieldUserPassword, 128));

            When(x => emailOptions.Value.SendMethod == EmailSendMethod.Grpc && !emailOptions.Value.Grpc.SendConfiguration, () =>
            {
                RuleFor(x => x.GrpcEmail).NotEmpty().WithMessage(string.Format(labelRequiredField, labelFieldGrpcMail));

                When(x => x.GrpcEmail != null, () =>
                {
                    RuleFor(x => x.GrpcEmail.TenancyName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelFieldGrpcMailTenancyName));
                    RuleFor(x => x.GrpcEmail.TenancyName).MaximumLength(128).WithMessage(string.Format(labelMaxLength, labelFieldGrpcMailTenancyName, 128));
                    RuleFor(x => x.GrpcEmail.UserName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelFieldGrpcMailUserName));
                    RuleFor(x => x.GrpcEmail.UserName).MaximumLength(128).WithMessage(string.Format(labelMaxLength, labelFieldGrpcMailUserName, 128));
                    RuleFor(x => x.GrpcEmail.Password).NotEmpty().WithMessage(string.Format(labelRequiredField, labelFieldGrpcMailPassword));
                    RuleFor(x => x.GrpcEmail.Password).MaximumLength(128).WithMessage(string.Format(labelMaxLength, labelFieldGrpcMailPassword, 128));
                });
            });
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
