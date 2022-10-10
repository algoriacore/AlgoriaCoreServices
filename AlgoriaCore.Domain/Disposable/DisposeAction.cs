using System;

namespace AlgoriaCore.Domain.Disposable
{
    public delegate void Void();

	public class DisposeAction : IDisposable
    {
		private readonly Void p;

        public DisposeAction(Void f)
        {
            this.p = f;
        }

		protected virtual void Dispose(bool disposing)
		{
			// Implementación del patrón IDisposable
			this.p();
		}

		public void Dispose()
        {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
