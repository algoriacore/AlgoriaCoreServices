using AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._3Commands;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Domain.Email;
using AlgoriaCore.Domain.Entities;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Multitenancy.Emails.Templates
{
    [Collection("TestsCollection")]
    public class MailTemplatesTests : TestBaseTenantDefault
    {
        public MailTemplatesTests(QueryTestFixture fixture) : base(fixture) { }

        #region SUCCESSFUL TESTS

        [Fact]
        public async Task CreateMailTemplateTest()
        {
            mailgroup mailGroup = Context.mailgroup.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.DisplayName == "DEFAULT");

            long response = await Mediator.Send(new MailTemplateCreateCommand()
            {
                MailGroup = mailGroup.Id,
                MailKey = EmailKeys.NewUser,
                DisplayName = "New User",
                SendTo = "responsablepruebas1@gmail.com",
                CopyTo = "etostado@algoria.com.mx",
                BlindCopyTo = "fbeltran@algoria.com.mx",
                Subject = "Default New",
                Body = "New User",

                IsActive = true
            });

            response.ShouldBeGreaterThan(0);

            mailtemplate entity = Context.mailtemplate.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == response);

            entity.ShouldNotBeNull();
        }

        #endregion
    }
}