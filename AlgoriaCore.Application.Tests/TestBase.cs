using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Session;
using AlgoriaPersistence;
using MediatR;
using UnitOfWork = AlgoriaCore.Application.Tests.Infrastructure.UnitOfWork;

namespace AlgoriaCore.Application.Tests
{
    public abstract class TestBase
    {
        protected IMediator Mediator;
        public IAppSession SessionContext;
        public UnitOfWork CurrentUnitOfWork;
        private readonly QueryTestFixture Fixture;
        protected AlgoriaCoreDbContext Context;
        public ICoreLogger Logger;

        protected TestBase(QueryTestFixture fixture)
        {
            Fixture = fixture;
            Mediator = fixture.Mediator;
            Context = fixture.Context;
            Logger = fixture.Logger;
        }

        public IAppSession GetSessionObject(int? tenantId, long userId)
        {
            return Fixture.GetSessionContext(tenantId, userId);
        }

        public IAppSession GetSessionContextHost()
        {
            return Fixture.GetSessionContextHost();
        }

        public IAppSession GetSessionContextTenantDefault()
        {
            return Fixture.GetSessionContextTenantDefault();
        }
    }
}

