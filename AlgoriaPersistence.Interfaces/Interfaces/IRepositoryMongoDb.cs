using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AlgoriaPersistence.Interfaces.Interfaces
{
    public interface IRepositoryMongoDb<TEntity> where TEntity : class
    {
        string GetCollectionName();
        IMongoCollection<TEntity> GetCollection();

        #region Count

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        Task<long> LongCountAsync();

        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Get

        IMongoQueryable<TEntity> GetAll();
        Task<List<TEntity>> GetAllListAsync();
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(string id);
        Task<TEntity> FirstOrDefaultAsync(string id);

        #endregion

        #region Insert

        Task<string> InsertAndGetIdAsync(TEntity entity);

        #endregion

        #region Update

        Task UpdateAsync(TEntity entity);

        #endregion

        #region Delete

        Task DeleteAsync(string id);

        #endregion
    }
}
