using AlgoriaPersistence;
using System;

namespace AlgoriaCore.Application.Tests.Infrastructure
{
    public class CommandTestBase : IDisposable
    {
        protected readonly AlgoriaCoreDbContext _context;

        public CommandTestBase()
        {
            _context = AlgoriaCoreContextFactory.Create();
        }

		protected virtual void Dispose(bool disposing)
		{
			// Implementación del patrón IDisposable
			AlgoriaCoreContextFactory.Destroy(_context);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}