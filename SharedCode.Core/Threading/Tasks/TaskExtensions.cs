namespace SharedCode.Threading.Tasks;

/// <summary>
/// The extension methods class for <see cref="Task"/>.
/// </summary>
[SuppressMessage(
    "Usage",
    "GCop500:Void async methods cannot be awaited. Also they can hide exceptions and cause tricky bugs. Return a Task instead.",
    Justification = "The whole point is to fire and forget without awaiting.")]
public static class TaskExtensions
{
    /// <summary>
    /// Safely execute the <see cref="Task"/> without waiting for it to complete before moving to
    /// the next line of code; commonly known as "Fire And Forget".
    /// </summary>
    /// <typeparam name="TException">The type of exception.</typeparam>
    /// <param name="task">The <see cref="Task"/>.</param>
    /// <param name="continueOnCapturedContext">
    /// If set to <c>true</c> continue on captured context; this will ensure that the
    /// Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a
    /// different context; this will allow the Synchronization Context to continue on a different thread.
    /// </param>
    /// <param name="onException">
    /// If an exception is thrown in the <see cref="Task"/>, <c>onException</c> will execute. If
    /// onException is null, the exception will be re-thrown.
    /// </param>
    /// <remarks>Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.</remarks>
    /// <exception cref="ArgumentNullException">task</exception>
    public static async void SafeFireAndForgetAsync<TException>(this Task task, bool continueOnCapturedContext = true, Action<TException>? onException = null)
        where TException : Exception
    {
        try
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(task);
#else
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }
#endif

            await task.ConfigureAwait(continueOnCapturedContext);
        }
        catch (TException ex) when (onException is not null)
        {
            onException!(ex);
        }
    }

    /// <summary>
    /// Safely execute the <see cref="Task"/> without waiting for it to complete before moving to
    /// the next line of code; commonly known as "Fire And Forget".
    /// </summary>
    /// <param name="task">The <see cref="Task"/>.</param>
    /// <param name="continueOnCapturedContext">
    /// If set to <c>true</c> continue on captured context; this will ensure that the
    /// Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a
    /// different context; this will allow the Synchronization Context to continue on a different thread.
    /// </param>
    /// <param name="onException">
    /// If an exception is thrown in the <see cref="Task"/>, <c>onException</c> will execute. If
    /// onException is null, the exception will be re-thrown.
    /// </param>
    /// <remarks>Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.</remarks>
    /// <exception cref="ArgumentNullException">task</exception>
    public static async void SafeFireAndForgetAsync(this Task task, bool continueOnCapturedContext = true, Action<Exception>? onException = null)
    {
        try
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(task);
#else
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }
#endif

            await task.ConfigureAwait(continueOnCapturedContext);
        }
        catch (Exception ex) when (onException is not null)
        {
            onException!(ex);
        }
    }
}
