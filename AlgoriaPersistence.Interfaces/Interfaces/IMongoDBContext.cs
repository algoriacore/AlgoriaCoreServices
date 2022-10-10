using MongoDB.Driver;

namespace AlgoriaPersistence.Interfaces.Interfaces
{
    public interface IMongoDBContext
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }
        public bool IsEnabled { get; }
        public bool IsActive { get; }

        void CheckConnection();
    }
}
