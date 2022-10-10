using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Date;
using AlgoriaCore.Application.QueriesAndCommands.ChangeLogs;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Domain.Entities;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Multitenancy.ChangeLogs
{
    [Collection("TestsCollection")]
    public class ChangeLogsTests : TestBaseTenantDefault
    {
        public ChangeLogsTests(QueryTestFixture fixture) : base(fixture) {}

        [Fact]
        public async Task GetChangeLogListTest()
        {
            Language language = Context.Language.FirstOrDefault();

            language.ShouldNotBeNull();

            string table = "Language";
            string key = language.Id.ToString();
            ChangeLog entity = new ChangeLog()
            {
                TenantId = CurrentUnitOfWork.GetTenantId(),
                key = key,
                table = table,
                UserId = SessionContext.UserId,
                datetime = new Clock().Now
            };

            entity.ChangeLogDetail.Add(new ChangeLogDetail() { data = "{{RecordUpdated}}" });

            Context.ChangeLog.Add(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            PagedResultDto<ChangeLogForListResponse> response = await Mediator.Send(new ChangeLogGetListQuery() { Table = table, Key = key });

            response.ShouldNotBeNull();
            response.Items.Count.ShouldBeGreaterThan(0);
        }
    }
}