using System;
using System.Threading.Tasks;

namespace AlgoriaPersistence.Interfaces.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDisposable SetTenantId(int? tenantId);
        int? GetTenantId();
        IDisposeBeginAction Begin();
        void Commit();
        void Rollback();

        IDisposable DisableFilter(string filter);
        bool HasFilter(string filter);

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
