using System;

namespace AlgoriaPersistence.Interfaces.Interfaces
{
    public interface IDisposeBeginAction : IDisposable
    {
        void Complete();

        void Rollback();

        bool IsCompleted();
    }
}
