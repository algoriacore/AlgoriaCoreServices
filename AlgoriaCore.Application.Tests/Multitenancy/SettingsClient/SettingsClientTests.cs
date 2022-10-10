using AlgoriaCore.Application.QueriesAndCommands.SettingsClient;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Domain.Entities;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Multitenancy.SettingsClient
{
    [Collection("TestsCollection")]
    public class SettingsClientTests : TestBaseTenantDefault
    {
        public SettingsClientTests(QueryTestFixture fixture) : base(fixture) {}

        [Fact]
        public async Task GetSettingsClientByClientTypeAndUserLoggedTest()
        {
            var settings = new[]
            {
                new SettingClient { TenantId = 1, UserId = SessionContext.UserId, ClientType = "web", Name = "timezonenew", value = "America/Chihuahua" }
            };

            Context.SettingClient.AddRange(settings);

            Context.SaveChanges();

            List<SettingClientResponse> response = await Mediator.Send(new SettingClientGetByClientTypeAndUserLoggedQuery() { ClientType = "web" });

            response.ShouldNotBeNull();
            response.Count.ShouldBeGreaterThan(0);
        }
    }
}