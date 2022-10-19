using AlgoriaCore.Domain.Interfaces;
using AlgoriaCore.Domain.Session;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AlgoriaPersistence.Repositories
{
    public class BaseRepository<TEntity, TType> : IRepository<TEntity, TType>
        where TEntity : class, IEntity<TType>
    {
        internal AlgoriaCoreDbContext Context;
        private readonly DbSet<TEntity> Table;
        internal IAppSession _sessionContext;
        private readonly IUnitOfWork _unitOfWork;

        public BaseRepository(AlgoriaCoreDbContext context,
                            IAppSession sessionContext,
                            IUnitOfWork unitOfwork)
        {
            this.Context = context;
            this.Table = context.Set<TEntity>();
            this._sessionContext = sessionContext;
            this._unitOfWork = unitOfwork;
        }

        #region Recuento

        public int Count()
        {
            return GetAll().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        public long LongCount()
        {
            return GetAll().LongCount();
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        public Task<long> LongCountAsync()
        {
            return Task.FromResult(LongCount());
        }

        public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LongCount(predicate));
        }

        #endregion

        #region Delete

        public void Delete(TEntity entity)
        {
            bool softDelete = SoftDelete();
            if (softDelete)
            {
                (entity as ISoftDelete).IsDeleted = true;
                Update(entity);
            }
            else
            {
                Table.Remove(entity);
            }
        }

        public void Delete(TType id)
        {
            var t = FirstOrDefault(id);
            if (t != null)
            {
                Delete(t);
            }
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }

        public Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }

        public Task DeleteAsync(TType id)
        {
            Delete(id);
            return Task.FromResult(0);
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                await DeleteAsync(entity);
            }
        }

        #endregion

        #region FirstOrDefault

        public TEntity FirstOrDefault(TType id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(TType id)
        {
            return await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        #endregion

        #region Get

        public TEntity Get(TType id)
        {
            return GetAll().First(CreateEqualityExpressionForId(id));
        }

        public IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = Table.AsQueryable();

            var parameter = Expression.Parameter(typeof(TEntity), "item");

            AddConditionHaveTenant(ref query, parameter);
            AddConditionSoftDelete(ref query, parameter);

            return query.AsQueryable();
        }

        private void AddConditionHaveTenant(ref IQueryable<TEntity> query, ParameterExpression parameter)
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

        private void AddConditionSoftDelete(ref IQueryable<TEntity> query, ParameterExpression parameter)
        {
            bool softDelete = SoftDelete() && _unitOfWork.HasFilter(AlgoriaCoreDataFilters.SoftDelete);
            if (softDelete)
            {
                var member = Expression.Property(parameter, "IsDeleted");
                var filter1 =
                        Expression.Constant(
                            Convert.ChangeType(true, member.Type.GetGenericArguments()[0]));

                Expression typeFilter = Expression.Convert(filter1, member.Type);
                var body = Expression.NotEqual(member, typeFilter);
                var predicateDelete = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

                if (predicateDelete != null)
                {
                    query = query.Where(predicateDelete);
                }
            }
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors == null)
            {
                return GetAll();
            }

            var query = GetAll();

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        public List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetAsync(TType id)
        {
            var entity = await GetAll().FirstAsync(CreateEqualityExpressionForId(id));

            return entity;
        }

        #endregion

        #region Insert

        public TEntity Insert(TEntity entity)
        {
            if (MustHaveTenant())
            {
                ((IMustHaveTenant)entity).TenantId = GetTenantId().Value;
            }
            else if (MayHaveTenant())
            {
                ((IMayHaveTenant)entity).TenantId = GetTenantId();
            }

            var r = Table.Add(entity);
            return r.Entity;
        }

        public TType InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        public Task<TType> InsertAndGetIdAsync(TEntity entity)
        {
            return Task.FromResult(InsertAndGetId(entity));
        }

        public Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public TEntity InsertOrUpdate(TEntity entity)
        {
            return entity.IsTransient()
                ? Insert(entity) : Update(entity);
        }

        public TType InsertOrUpdateAndGetId(TEntity entity)
        {
            return InsertOrUpdate(entity).Id;
        }

        public Task<TType> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            return Task.FromResult(InsertOrUpdateAndGetId(entity));
        }

        public async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return entity.IsTransient()
                 ? await InsertAsync(entity) : await UpdateAsync(entity);
        }

        #endregion

        #region Update

        public TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public TEntity Update(TType id, Action<TEntity> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);
            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        public async Task<TEntity> UpdateAsync(TType id, Func<TEntity, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return entity;
        }

        #endregion

        public TEntity Load(TType id)
        {
            return Get(id);
        }

        public T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        public string GetTableName()
        {
            return Context.Model.FindEntityType(typeof(TEntity)).GetTableName();
        }

        public List<string> GetColumnNames()
        {
            return Context.Model.FindEntityType(typeof(TEntity))
                           .GetProperties().Select(x => x.GetColumnName())
                           .ToList();
        }

        protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TType id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TType))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
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
    }
}
