using AlgoriaCore.Application.QueriesAndCommands.SettingsClient;
using AlgoriaCore.Application.Tests.Infrastructure;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Host.SettingsClient
{
    [Collection("TestsCollection")]
    public class SettingsClientTests : TestBaseHost
    {
        public SettingsClientTests(QueryTestFixture fixture) : base(fixture) {}

        [Fact]
        public async Task GetSettingsClientByClientTypeAndUserLoggedTest()
        {
            List<SettingClientResponse> response = await Mediator.Send(new SettingClientGetByClientTypeAndUserLoggedQuery() { ClientType = "web" });

            response.ShouldNotBeNull();
            response.Count.ShouldBeGreaterThan(0);
        }
    }
}