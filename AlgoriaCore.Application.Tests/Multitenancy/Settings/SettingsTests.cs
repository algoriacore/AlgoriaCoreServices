using AlgoriaCore.Application.QueriesAndCommands.Settings.Tenant;
using AlgoriaCore.Application.Tests.ASanitization;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Extensions;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Multitenancy.Settings
{
    [Collection("TestsCollection")]
    public class SettingsTests : TestBaseTenantDefault
    {
        public SettingsTests(QueryTestFixture fixture) : base(fixture) {}

        #region SUCCESSFUL TESTS

        [Fact]
        public async Task UpdateSettingsTest()
        {
            TenantSettingsForEditResponse responseGetForEdit = await Mediator.Send(new TenantSettingsGetForEditQuery());

            responseGetForEdit.ShouldNotBeNull();

            bool preventConcurrentSesions = !responseGetForEdit.PreventConcurrentSesions;

            var dto = new TenantSettingsUpdateCommand();

            dto.PasswordUseDefaultConfiguration = responseGetForEdit.PasswordUseDefaultConfiguration;
            dto.PasswordComplexity = new TenantSettingsPasswordComplexityCommand()
            {
                MinimumLength = responseGetForEdit.PasswordComplexity.MinimumLength,
                MaximumLength = responseGetForEdit.PasswordComplexity.MaximumLength,
                UseNumbers = responseGetForEdit.PasswordComplexity.UseNumbers,
                UseUppercase = responseGetForEdit.PasswordComplexity.UseUppercase,
                UseLowercase = responseGetForEdit.PasswordComplexity.UseLowercase,
                UsePunctuationSymbols = responseGetForEdit.PasswordComplexity.UsePunctuationSymbols
            };

            dto.FailedAttemptsToBlockUser = 3;
            dto.UserBlockingDuration = 5;
            dto.EnablePasswordReuseValidation = responseGetForEdit.EnablePasswordReuseValidation;
            dto.EnablePasswordPeriod = responseGetForEdit.EnablePasswordPeriod;
            dto.PasswordValidDays = responseGetForEdit.PasswordValidDays;
			dto.EnableUserBlocking = responseGetForEdit.EnableUserBlocking;
            dto.PreventConcurrentSesions = preventConcurrentSesions;
            dto.MailSMTPSenderDefault = responseGetForEdit.MailSMTPSenderDefault;
            dto.MailSMTPSenderDefaultDisplayName = responseGetForEdit.MailSMTPSenderDefaultDisplayName;
            dto.MailSMTPHost = responseGetForEdit.MailSMTPHost;
            dto.MailSMTPPort = responseGetForEdit.MailSMTPPort;
            dto.MailEnableSSL = responseGetForEdit.MailEnableSSL;
            dto.MailUseDefaultCredentials = responseGetForEdit.MailUseDefaultCredentials;
            dto.MailDomainName = responseGetForEdit.MailDomainName;
            dto.MailUserName = responseGetForEdit.MailUserName;
            dto.MailUserPassword = responseGetForEdit.MailUserPassword;

            var response = await Mediator.Send(dto);

            response.ShouldBe(0);

            responseGetForEdit = await Mediator.Send(new TenantSettingsGetForEditQuery());

            responseGetForEdit.ShouldNotBeNull();
            responseGetForEdit.PreventConcurrentSesions.ShouldBe(preventConcurrentSesions);
        }

        [Fact]
        public async Task GetSettingsForEditTest()
        {
            TenantSettingsForEditResponse response = await Mediator.Send(new TenantSettingsGetForEditQuery());

            response.ShouldNotBeNull();
        }

        [Fact]
        public async Task SendTestEmailTest()
        {
            int response = await Mediator.Send(new TenantSettingsSendTestEmailCommand() { EmailAddress = "responsablepruebas1@gmail.com" } );

            response.ShouldBe(0);
        }

        #endregion

        #region FAILED TESTS

        [Fact]
        public async Task UpdateSettingsSanitizeTest()
        {
            TenantSettingsForEditResponse responseGetForEdit = await Mediator.Send(new TenantSettingsGetForEditQuery());

            responseGetForEdit.ShouldNotBeNull();

            var ac = new ASanitizeTest<TenantSettingsUpdateCommand, int>(new TenantSettingsUpdateCommand()
            {
                PasswordUseDefaultConfiguration = responseGetForEdit.PasswordUseDefaultConfiguration,
                PasswordComplexity = new TenantSettingsPasswordComplexityCommand()
                {
                    MinimumLength = responseGetForEdit.PasswordComplexity.MinimumLength,
                    MaximumLength = responseGetForEdit.PasswordComplexity.MaximumLength,
                    UseNumbers = responseGetForEdit.PasswordComplexity.UseNumbers,
                    UseUppercase = responseGetForEdit.PasswordComplexity.UseUppercase,
                    UseLowercase = responseGetForEdit.PasswordComplexity.UseLowercase,
                    UsePunctuationSymbols = responseGetForEdit.PasswordComplexity.UsePunctuationSymbols
                },
                EnablePasswordReuseValidation = responseGetForEdit.EnablePasswordReuseValidation,
                EnablePasswordPeriod = responseGetForEdit.EnablePasswordPeriod,
                PasswordValidDays = responseGetForEdit.PasswordValidDays,
                EnableUserBlocking = responseGetForEdit.EnableUserBlocking,
                FailedAttemptsToBlockUser = responseGetForEdit.FailedAttemptsToBlockUser,
                UserBlockingDuration = responseGetForEdit.UserBlockingDuration,
                PreventConcurrentSesions = responseGetForEdit.PreventConcurrentSesions,
                MailSMTPSenderDefault = responseGetForEdit.MailSMTPSenderDefault,
                MailSMTPSenderDefaultDisplayName = responseGetForEdit.MailSMTPSenderDefaultDisplayName,
                MailSMTPHost = responseGetForEdit.MailSMTPHost,
                MailSMTPPort = responseGetForEdit.MailSMTPPort,
                MailEnableSSL = responseGetForEdit.MailEnableSSL,
                MailUseDefaultCredentials = responseGetForEdit.MailUseDefaultCredentials,
                MailDomainName = responseGetForEdit.MailDomainName,
                MailUserName = responseGetForEdit.MailUserName,
                MailUserPassword = responseGetForEdit.MailUserPassword
            });

            ac.Mediator = Mediator;

            if (!responseGetForEdit.PasswordUseDefaultConfiguration)
            {
                ac.RuleFor(x => x.PasswordComplexity.MinimumLength).GreaterThan(0);
                ac.RuleFor(x => x.PasswordComplexity.MaximumLength).GreaterThan(0);
            }

            ac.RuleFor(x => x.PasswordValidDays).GreaterThanOrEqualTo(0);

            if (responseGetForEdit.EnableUserBlocking)
            {
                ac.RuleFor(x => x.FailedAttemptsToBlockUser).GreaterThan(0);
                ac.RuleFor(x => x.UserBlockingDuration).GreaterThan(0);
            }

            ac.RuleFor(x => x.MailSMTPSenderDefault).MaxLength(256);
            ac.RuleFor(x => x.MailSMTPSenderDefault).EmailAddress();
            ac.RuleFor(x => x.MailSMTPSenderDefaultDisplayName).MaxLength(128);
            ac.RuleFor(x => x.MailSMTPHost).MaxLength(64);
            ac.RuleFor(x => x.MailSMTPPort).GreaterThanOrEqualTo(0);

            if (!responseGetForEdit.MailDomainName.IsNullOrWhiteSpace())
            {
                ac.RuleFor(x => x.MailDomainName).MaxLength(128);
            }

            if (!responseGetForEdit.MailUseDefaultCredentials)
            {
                ac.RuleFor(x => x.MailUserName).NotEmpty();
                ac.RuleFor(x => x.MailUserName).MaxLength(128);
                ac.RuleFor(x => x.MailUserPassword).NotEmpty();
                ac.RuleFor(x => x.MailUserPassword).MaxLength(128);
            }
            
            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }
        }

        #endregion
    }
}