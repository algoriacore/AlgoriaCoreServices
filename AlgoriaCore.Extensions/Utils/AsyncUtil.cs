using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Extensions.Utils
{
    /// <summary>
    /// Clase utilitaria para ejecutar métodos asíncronos dentro de un proceso síncrono.
    /// </summary>
    public static class AsyncUtil
    {
        private static readonly TaskFactory _taskFactory = new
            TaskFactory(CancellationToken.None,
                        TaskCreationOptions.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.Default);

        /// <summary>
        /// Ejecuta una método de tarea asíncrona, el cual tiene un valor de retorno vacío síncrono
        /// USAGE: AsyncUtil.RunSync(() => AsyncMethod());
        /// </summary>
        /// <param name="task">Método de tarea para ejecutar</param>
        public static void RunSync(Func<Task> task)
            => _taskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// Ejecuta un método asíncrono Task<T> el cual tien un tipo de retorno T síncrono
        /// USAGE: T result = AsyncUtil.RunSync(() => AsyncMethod<T>());
        /// </summary>
        /// <typeparam name="TResult">Tipo de retorno</typeparam>
        /// <param name="task">Método Task<T> a ejecutar</param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> task)
            => _taskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
    }
}
