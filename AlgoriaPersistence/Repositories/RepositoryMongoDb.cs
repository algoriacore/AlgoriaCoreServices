using AlgoriaCore.Domain.Attributes;
using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using AlgoriaPersistence.Interfaces.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AlgoriaPersistence.Repositories
{
    public class RepositoryMongoDb<TEntity> : IRepositoryMongoDb<TEntity> where TEntity : MongoDbEntity
    {
        internal IMongoDBContext _context;
        private readonly IMongoCollection<TEntity> _collection;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;

        public RepositoryMongoDb(IMongoDBContext context, IUnitOfWork unitOfwork, IMongoUnitOfWork mongoUnitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfwork;
            _mongoUnitOfWork = mongoUnitOfWork;

            _collection = context.Database.GetCollection<TEntity>(GetCollectionName());
        }

        #region Count

        public async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        public async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        #endregion

        #region Get

        public IMongoQueryable<TEntity> GetAll()
        {
            IMongoQueryable<TEntity> query = _collection.AsQueryable(_mongoUnitOfWork.GetSession());

            var parameter = Expression.Parameter(typeof(TEntity), "item");

            AddConditionHaveTenant(ref query, parameter);

            return query;
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetAsync(string id)
        {
            return await GetAll().FirstAsync(p => p.Id == id);
        }

        public async Task<TEntity> FirstOrDefaultAsync(string id)
        {
            return await GetAll().FirstOrDefaultAsync(p => p.Id == id);
        }

        #endregion

        #region Insert

        public async Task<string> InsertAndGetIdAsync(TEntity entity)
        {
            if (MustHaveTenant())
            {
                ((IMustHaveTenant)entity).TenantId = GetTenantId().Value;
            }
            else if (MayHaveTenant())
            {
                ((IMayHaveTenant)entity).TenantId = GetTenantId();
            }

            await _collection.InsertOneAsync(_mongoUnitOfWork.GetSession(), entity);

            return entity.Id;
        }

        #endregion

        #region Update

        public async Task UpdateAsync(TEntity entity)
        {
            await _collection.ReplaceOneAsync(_mongoUnitOfWork.GetSession(), p => p.Id == entity.Id, entity);
        }

        #endregion

        #region Delete

        public async Task DeleteAsync(TEntity entity)
        {
            bool softDelete = SoftDelete();

            if (softDelete)
            {
                (entity as ISoftDelete).IsDeleted = true;

                await UpdateAsync(entity);
            }
            else
            {
                await _collection.DeleteOneAsync(_mongoUnitOfWork.GetSession(), p => p.Id == entity.Id);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var t = await FirstOrDefaultAsync(id);

            if (t != null)
            {
                await DeleteAsync(t);
            }
        }

        #endregion

        public string GetCollectionName()
        {
            return (typeof(TEntity).GetCustomAttributes(typeof(BsonCollectionNameAttribute), true).FirstOrDefault()
                as BsonCollectionNameAttribute).CollectionName;
        }

        public IMongoCollection<TEntity> GetCollection()
        {
            return _collection;
        }

        private bool MustHaveTenant()
        {
            return typeof(TEntity).GetInterface("imusthavetenant", true) != null;
        }

        private bool MayHaveTenant()
        {
            return typeof(TEntity).GetInterface("imayhavetenant", true) != null;
        }

        private bool SoftDelete()
        {
            return typeof(TEntity).GetInterface("isoftdelete", true) != null;
        }

        private int? GetTenantId()
        {
            return _unitOfWork.GetTenantId();
        }

        private void AddConditionHaveTenant(ref IMongoQueryable<TEntity> query, ParameterExpression parameter)
        {
            bool mustHaveTenant = MustHaveTenant() && _unitOfWork.HasFilter(AlgoriaCoreDataFilters.MustHaveTenant);
            bool mayHaveTenant = MayHaveTenant() && _unitOfWork.HasFilter(AlgoriaCoreDataFilters.MayHaveTenant);
            var tId = GetTenantId();

            if (mustHaveTenant || mayHaveTenant)
            {
                Expression<Func<TEntity, bool>> predicateTenant = null;
                var prop = typeof(TEntity).GetProperty("TenantId");
                var member = Expression.Property(parameter, "TenantId");

                if (mustHaveTenant)
                {
                    var constant = Expression.Constant(Convert.ChangeType(tId, prop.PropertyType));
                    var body = Expression.Equal(member, constant);
                    predicateTenant = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
                }

                if (mayHaveTenant)
                {
                    if (tId != null)
                    {
                        var filter1 =
                                Expression.Constant(
                                    Convert.ChangeType(tId, member.Type.GetGenericArguments()[0]));

                        Expression typeFilter = Expression.Convert(filter1, member.Type);
                        var body = Expression.Equal(member, typeFilter);
                        predicateTenant = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
                    }
                    else
                    {
                        var constant = Expression.Constant(null);
                        var body = Expression.Equal(member, constant);
                        predicateTenant = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
                    }
                }

                if (predicateTenant != null)
                {
                    query = query.Where(predicateTenant);
                }
            }
        }
    }
}
