using AlgoriaPersistence.Interfaces.Interfaces;
using System;

namespace AlgoriaPersistence.UOW
{
    public class DisposeBeginAction : IDisposeBeginAction
    {
        private readonly IUnitOfWork _parentUnitOfWork;
        private bool completed = false;

        public DisposeBeginAction(IUnitOfWork parentUnitOrWork)
        {
            _parentUnitOfWork = parentUnitOrWork;
        }

        public void Complete()
        {
            _parentUnitOfWork.Commit();
            completed = true;
        }

        public void Rollback()
        {
            _parentUnitOfWork.Rollback();
            completed = true;
        }

        public bool IsCompleted()
        {
            return completed;
        }

        protected virtual void Dispose(bool disposing)
        {
            // Implementación del patrón IDisposable
            if (!completed)
            {
                Rollback();
            }

            _parentUnitOfWork.Close();
        }

        public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
