using System;
using System.Threading.Tasks;

namespace AlgoriaPersistence.Interfaces.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDisposable SetTenantId(int? tenantId);
        int? GetTenantId();
        IDisposable SetUserId(long? userId);
        long? GetUserId();
        IDisposeBeginAction Begin();
        void Commit();
        void Rollback();
        void Close();

        IDisposable DisableFilter(string filter);
        bool HasFilter(string filter);

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
