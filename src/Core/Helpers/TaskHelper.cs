using System;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.Core.Helpers
{
    /// <summary>
    /// Contains various helper methods for working with tasks.
    /// </summary>
    public static class TaskHelper
    {
        private static readonly TaskFactory Factory = new TaskFactory(
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);

        /// <summary>
        /// Runs a generic task synchronously.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The task wrapped as a func.</param>
        /// <returns>The result of the task</returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func) =>
            Factory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// Runs a non-generic task synchronously.
        /// </summary>
        /// <param name="func">The task wrapped as a func.</param>
        public static void RunSync(Func<Task> func) =>
            Factory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
    }
}