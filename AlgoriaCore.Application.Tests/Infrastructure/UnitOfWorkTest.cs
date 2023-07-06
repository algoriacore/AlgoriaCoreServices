using AlgoriaCore.Domain.Disposable;
using AlgoriaCore.Domain.Session;
using AlgoriaPersistence;
using AlgoriaPersistence.Interfaces.Interfaces;
using AlgoriaPersistence.UOW;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Tests.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        internal AlgoriaCoreDbContext Context;
        private readonly IAppSession _sessionContext;

        private IDbContextTransaction dbTransaction;

        private int? tenantId;
        private readonly List<int?> beforeTenantId;

        private long? userId;
        private readonly List<long?> beforeUserId;

        private readonly List<string> EnabledFilters = null;
        private readonly List<string> _filterDisabilizations = null;
        private DisposeBeginAction _disposeBeginAction = null;

        public UnitOfWork(AlgoriaCoreDbContext context,
                        IAppSession sessionContext
                        )
        {
            this.Context = context;
            this._sessionContext = sessionContext;

            this.beforeTenantId = new List<int?>();
            this.beforeUserId = new List<long?>();
            this._filterDisabilizations = new List<string>();

            EnabledFilters = new List<string>();
            EnabledFilters.Add(AlgoriaCoreDataFilters.MayHaveTenant);
            EnabledFilters.Add(AlgoriaCoreDataFilters.MustHaveTenant);
            EnabledFilters.Add(AlgoriaCoreDataFilters.SoftDelete);
        }

        public IDisposeBeginAction Begin()
        {
            if (_disposeBeginAction != null && !_disposeBeginAction.IsCompleted() && dbTransaction != null)
            {
                return _disposeBeginAction;
            }
            else
            {
                dbTransaction = Context.Database.BeginTransaction();
                _disposeBeginAction = new DisposeBeginAction(this);

                return _disposeBeginAction;
            }
        }

        public void Commit()
        {
            if (_disposeBeginAction != null && !_disposeBeginAction.IsCompleted() && dbTransaction != null)
            {
                dbTransaction.Commit();
                dbTransaction = null;
            }
        }

        public void Rollback()
        {
            if (_disposeBeginAction != null && !_disposeBeginAction.IsCompleted() && dbTransaction != null) // dbTransaction != null)
            {
                dbTransaction.Rollback();
                dbTransaction = null;
            }
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public IDisposable SetTenantId(int? tenantId)
        {
            // Saves before tenant ID
            if (beforeTenantId.Count <= 0)
            {
                this.tenantId = _sessionContext.TenantId;
            }

            beforeTenantId.Add(this.tenantId);

            this.tenantId = tenantId;

            return new DisposeAction(() =>
            {
                // Se recupera el último tenantId encolado y después se elimina de la lista
                if (beforeTenantId.Count > 0)
                {
                    this.tenantId = beforeTenantId[beforeTenantId.Count - 1];
                    beforeTenantId.RemoveAt(beforeTenantId.Count - 1);
                }
                else
                {
                    this.tenantId = _sessionContext.TenantId;
                }
            });
        }

        public int? GetTenantId()
        {
            if (beforeTenantId.Count > 0)
            {
                return tenantId;
            }
            else
            {
                return tenantId.HasValue ? tenantId : _sessionContext.TenantId;
            }
        }

        public IDisposable SetUserId(long? userId)
        {
            // Saves before user ID
            if (beforeUserId.Count <= 0)
            {
                this.userId = _sessionContext.UserId;
            }

            beforeUserId.Add(this.userId);

            this.userId = userId;

            return new DisposeAction(() =>
            {
                // Se recupera el último tenantId encolado y después se elimina de la lista
                if (beforeUserId.Count > 0)
                {
                    this.userId = beforeUserId[beforeUserId.Count - 1];
                    beforeUserId.RemoveAt(beforeUserId.Count - 1);
                }
                else
                {
                    this.userId = _sessionContext.UserId;
                }
            });
        }

        public long? GetUserId()
        {
            if (beforeUserId.Count > 0)
            {
                return userId;
            }
            else
            {
                return userId.HasValue ? userId : _sessionContext.UserId;
            }
        }

        public IDisposable DisableFilter(string filter)
        {
            // Se agrega el filtro indicado a la lista de filtros deshabilitados
            _filterDisabilizations.Add(filter);

            // Se elimina de la lista de 
            EnabledFilters.Remove(filter);

            return new DisposeAction(() => {

                // Se busca una ocurrencia del filtro indicado en la lista de "deshabilitizaciones" de filtros
                // Si se encuentra en esa lista, entonces se elimina una ocurrencia.
                // Al final, si no queda ya ningua ocurrencia del filtro actual en la lista de deshabilitizaciones, entonces 
                // se habilita el filtro.
                // Esto hace para que funcione con el anidamiento de desactivaciones de filtros
                var ds = _filterDisabilizations.FirstOrDefault(m => m == filter);
                if (ds != null)
                {
                    _filterDisabilizations.Remove(ds);
                }

                if (!_filterDisabilizations.Any(m => m == filter))
                {
                    EnabledFilters.Add(filter);
                }
            });
        }

        public bool HasFilter(string filter)
        {
            return EnabledFilters.IndexOf(filter) >= 0;
        }

        protected virtual void Dispose(bool disposing)
        {
            // Implementación del patrón IDisposable
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
