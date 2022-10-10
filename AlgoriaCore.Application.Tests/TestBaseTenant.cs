using AlgoriaCore.Application.Tests.Infrastructure;

namespace AlgoriaCore.Application.Tests
{
    public abstract class TestBaseTenantDefault : TestBase
    {
        protected TestBaseTenantDefault(QueryTestFixture fixture): base(fixture)
        {
            SessionContext = fixture.GetSessionContextTenantDefault();

            CurrentUnitOfWork = fixture.GetUnitOfWork();
            CurrentUnitOfWork.SetTenantId(SessionContext.TenantId);
        }
    }
}
