using System;
using System.Threading;
using System.Threading.Tasks;

namespace Networking.Framework.Utils
{
    /// <summary>
    /// Extensions for <see cref="Task"/>
    /// </summary>
    public static class TaskExt
    {
        /// <summary>
        /// Wait for either the task to complete or for a cancellation request to arrive
        /// </summary>
        /// <exception cref="OperationCanceledException"></exception>
        public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>) s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
                await task;
            }
        }
    }
}