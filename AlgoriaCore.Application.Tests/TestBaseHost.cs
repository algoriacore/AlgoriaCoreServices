using AlgoriaCore.Application.Tests.Infrastructure;

namespace AlgoriaCore.Application.Tests
{
    public abstract class TestBaseHost: TestBase
    {
        protected TestBaseHost(QueryTestFixture fixture): base(fixture)
        {
            SessionContext = fixture.GetSessionContextHost();

            CurrentUnitOfWork = fixture.GetUnitOfWork();
            CurrentUnitOfWork.SetTenantId(SessionContext.TenantId);
        }
    }
}
