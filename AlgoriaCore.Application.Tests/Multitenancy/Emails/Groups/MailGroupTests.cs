using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands;
using AlgoriaCore.Application.Tests.ASanitization;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Settings;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Multitenancy.Emails.Groups
{
    [Collection("TestsCollection")]
    public class MailGroupTests : TestBaseTenantDefault
    {
        public MailGroupTests(QueryTestFixture fixture) : base(fixture) { }

        #region SUCCESSFUL TESTS

        [Fact]
        public async Task CreateMailGroupTest()
        {
            long response = await Mediator.Send(new MailGroupCreateCommand()
            {
                DisplayName = "TEST GROUP",
                Header = "-----Header-----",
                Footer = "-----Footer-----"
            });

            response.ShouldBeGreaterThan(0);

            mailgroup entity = Context.mailgroup.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == response);

            entity.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateMailGroupTest()
        {
            mailgroup entity = Context.mailgroup.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.DisplayName.ToUpper() == "UPD");
            string header = "-----Header MOD-----";
            string footer = "-----Footer MOD-----";

            long response = await Mediator.Send(new MailGroupUpdateCommand()
            {
                Id = entity.Id,
                DisplayName = entity.DisplayName,
                Header = header,
                Footer = footer
            });

            response.ShouldBeGreaterThan(0);

            entity = Context.mailgroup.FirstOrDefault(p => p.Id == entity.Id);

            entity.mailgrouptxt.FirstOrDefault(p => p.type == 1).body.ShouldBe(header);
            entity.mailgrouptxt.FirstOrDefault(p => p.type == 2).body.ShouldBe(footer);
        }

        [Fact]
        public async Task CheckMailGroupTest()
        {
            mailgroup entity = Context.mailgroup.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.DisplayName.ToUpper() == "UPD");

            long response = await Mediator.Send(new MailGroupCheckCommand()
            {
                Id = entity.Id
            });

            response.ShouldBeGreaterThan(0);

            Setting setting = Context.Setting.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name == AppSettings.General.MailGroup);

            setting.value.ShouldBe(response.ToString());
        }

        [Fact]
        public async Task UncheckMailGroupTest()
        {
            mailgroup entity = Context.mailgroup.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.DisplayName.ToUpper() == "UPD");

            long response = await Mediator.Send(new MailGroupCheckCommand()
            {
                Id = entity.Id
            });

            Setting setting = Context.Setting.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name == AppSettings.General.MailGroup);

            setting.value.ShouldBe(response.ToString());

            await Mediator.Send(new MailGroupUnCheckCommand()
            {
                Id = entity.Id
            });

            setting = Context.Setting.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name == AppSettings.General.MailGroup);

            setting.value.ShouldBeNull();

            entity = Context.mailgroup.FirstOrDefault(p => p.TenantId == SessionContext.TenantId);

            long responseCheck = await Mediator.Send(new MailGroupCheckCommand()
            {
                Id = entity.Id
            });

            setting = Context.Setting.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name == AppSettings.General.MailGroup);

            setting.value.ShouldBe(responseCheck.ToString());
        }

        [Fact]
        public async Task CopyMailGroupTest()
        {
            mailgroup entity = Context.mailgroup.FirstOrDefault(p => p.TenantId == SessionContext.TenantId);
            string displayName = entity.DisplayName + " COPY";

            long response = await Mediator.Send(new MailGroupCopyCommand()
            {
                Id = entity.Id,
                DisplayName = displayName
            });

            response.ShouldBeGreaterThan(0);

            entity = Context.mailgroup.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == response);

            entity.ShouldNotBeNull();
            entity.DisplayName.ShouldBe(displayName);
        }

        [Fact]
        public async Task GetMailGroupListTest()
        {
            PagedResultDto<MailGroupListResponse> response = await Mediator.Send(new MailGroupGetListQuery());

            response.ShouldNotBeNull();
            response.Items.Count.ShouldBeGreaterThan(0);
        }

        #endregion

        #region FAILED TESTS

        [Fact]
        public async Task CreateMailGroupSanitize()
        {
            var ac = new ASanitizeTest<MailGroupCreateCommand, long>(new MailGroupCreateCommand()
            {
                DisplayName = "TEST GROUP",
                Header = "-----Header-----",
                Footer = "-----Footer-----"
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.DisplayName).NotEmpty();
            ac.RuleFor(x => x.DisplayName).MaxLength(100);

            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }

			Assert.Equal(0, ac.TotalErrors);
		}

        [Fact]
        public async Task UpdateMailGroupSanitize()
        {
            mailgroup entity = Context.mailgroup.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.DisplayName.ToUpper() == "UPD");

            var ac = new ASanitizeTest<MailGroupUpdateCommand, long>(new MailGroupUpdateCommand()
            {
                Id = entity.Id,
                DisplayName = entity.DisplayName,
                Header = "-----Header-----",
                Footer = "-----Footer-----"
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.Id).NotEmpty(); 
            ac.RuleFor(x => x.DisplayName).NotEmpty();
            ac.RuleFor(x => x.DisplayName).MaxLength(100);

            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }

			Assert.Equal(0, ac.TotalErrors);
		}

        #endregion
    }
}