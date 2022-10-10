using MongoDB.Driver;

namespace AlgoriaPersistence.Interfaces.Interfaces
{
    public interface IMongoUnitOfWork
    {
        void Begin();
        void Commit();
        void Rollback();
        IClientSessionHandle GetSession();
    }
}
