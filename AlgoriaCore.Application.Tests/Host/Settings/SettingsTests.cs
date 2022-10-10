using AlgoriaCore.Application.QueriesAndCommands.Settings.Host;
using AlgoriaCore.Application.Tests.ASanitization;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Extensions;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Host.Settings
{
    [Collection("TestsCollection")]
    public class SettingsTests : TestBaseHost
    {
        public SettingsTests(QueryTestFixture fixture) : base(fixture) {}

        #region SUCCESSFUL TESTS

        [Fact]
        public async Task UpdateSettingsTest()
        {
            HostSettingsForEditResponse responseGetForEdit = await Mediator.Send(new HostSettingsGetForEditQuery());

            responseGetForEdit.ShouldNotBeNull();

            bool enableUserBlocking = true;

            int response = await Mediator.Send(new HostSettingsUpdateCommand()
            {
                WebSiteRootAddress = responseGetForEdit.WebSiteRootAddress,
                PasswordUseDefaultConfiguration = responseGetForEdit.PasswordUseDefaultConfiguration,
                PasswordComplexity = new HostSettingsPasswordComplexityCommand()
                {
                    MinimumLength = responseGetForEdit.PasswordComplexity.MinimumLength,
                    MaximumLength = responseGetForEdit.PasswordComplexity.MaximumLength,
                    UseNumbers = responseGetForEdit.PasswordComplexity.UseNumbers,
                    UseUppercase = responseGetForEdit.PasswordComplexity.UseUppercase,
                    UseLowercase = responseGetForEdit.PasswordComplexity.UseLowercase,
                    UsePunctuationSymbols = responseGetForEdit.PasswordComplexity.UsePunctuationSymbols
                },
                EnableUserBlocking = enableUserBlocking,
                FailedAttemptsToBlockUser = responseGetForEdit.FailedAttemptsToBlockUser,
                UserBlockingDuration = responseGetForEdit.UserBlockingDuration,
                EnableTwoFactorLogin = responseGetForEdit.EnableTwoFactorLogin,
                EnableMailVerification = responseGetForEdit.EnableMailVerification,
                EnableSMSVerification = responseGetForEdit.EnableSMSVerification,
                EnableBrowserRemenberMe = responseGetForEdit.EnableBrowserRemenberMe,
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

            response.ShouldBe(0);

            responseGetForEdit = await Mediator.Send(new HostSettingsGetForEditQuery());

            responseGetForEdit.ShouldNotBeNull();
            responseGetForEdit.EnableUserBlocking.ShouldBe(enableUserBlocking);
        }

        [Fact]
        public async Task GetSettingsForEditTest()
        {
            HostSettingsForEditResponse response = await Mediator.Send(new HostSettingsGetForEditQuery());

            response.ShouldNotBeNull();
        }

        [Fact]
        public async Task SendTestEmailTest()
        {
            int response = await Mediator.Send(new HostSettingsSendTestEmailCommand() { EmailAddress = "responsablepruebas1@gmail.com" } );

            response.ShouldBe(0);
        }

        #endregion

        #region FAILED TESTS

        [Fact]
        public async Task UpdateSettingsSanitizeTest()
        {
            HostSettingsForEditResponse responseGetForEdit = await Mediator.Send(new HostSettingsGetForEditQuery());

            responseGetForEdit.ShouldNotBeNull();

            var ac = new ASanitizeTest<HostSettingsUpdateCommand, int>(new HostSettingsUpdateCommand()
            {
                WebSiteRootAddress = responseGetForEdit.WebSiteRootAddress,
                PasswordUseDefaultConfiguration = responseGetForEdit.PasswordUseDefaultConfiguration,
                PasswordComplexity = new HostSettingsPasswordComplexityCommand()
                {
                    MinimumLength = responseGetForEdit.PasswordComplexity.MinimumLength,
                    MaximumLength = responseGetForEdit.PasswordComplexity.MaximumLength,
                    UseNumbers = responseGetForEdit.PasswordComplexity.UseNumbers,
                    UseUppercase = responseGetForEdit.PasswordComplexity.UseUppercase,
                    UseLowercase = responseGetForEdit.PasswordComplexity.UseLowercase,
                    UsePunctuationSymbols = responseGetForEdit.PasswordComplexity.UsePunctuationSymbols
                },
                EnableUserBlocking = responseGetForEdit.EnableUserBlocking,
                FailedAttemptsToBlockUser = responseGetForEdit.FailedAttemptsToBlockUser,
                UserBlockingDuration = responseGetForEdit.UserBlockingDuration,
                EnableTwoFactorLogin = responseGetForEdit.EnableTwoFactorLogin,
                EnableMailVerification = responseGetForEdit.EnableMailVerification,
                EnableSMSVerification = responseGetForEdit.EnableSMSVerification,
                EnableBrowserRemenberMe = responseGetForEdit.EnableBrowserRemenberMe,
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

            ac.RuleFor(x => x.WebSiteRootAddress).NotEmpty();
            ac.RuleFor(x => x.WebSiteRootAddress).MaxLength(128);

            if (!responseGetForEdit.PasswordUseDefaultConfiguration)
            {
                ac.RuleFor(x => x.PasswordComplexity.MinimumLength).GreaterThan(0);
                ac.RuleFor(x => x.PasswordComplexity.MaximumLength).GreaterThan(0);
            }

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