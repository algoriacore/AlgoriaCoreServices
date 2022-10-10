
using AlgoriaCore.Domain.Interfaces;

namespace AlgoriaPersistence.Interfaces.Interfaces
{
    public interface IRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : class, IEntity<int>
    {

    }
}
