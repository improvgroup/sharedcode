namespace SharedCode.Threading.Tasks;

/// <summary>
/// The extension methods class for <see cref="ValueTask" />.
/// </summary>
public static class ValueTaskExtensions
{
	/// <summary>
	/// Safely execute the <see cref="ValueTask" /> without waiting for it to complete before
	/// moving to the next line of code; commonly known as "Fire And Forget".
	/// </summary>
	/// <typeparam name="TException">The type of exception.</typeparam>
	/// <param name="task">The <see cref="ValueTask" />.</param>
	/// <param name="continueOnCapturedContext">
	/// If set to <c>true</c> continue on captured context; this will ensure that the
	/// Synchronization Context returns to the calling thread. If set to <c>false</c> continue
	/// on a different context; this will allow the Synchronization Context to continue on a
	/// different thread.
	/// </param>
	/// <param name="onException">
	/// If an exception is thrown in the <see cref="ValueTask" />, <c>onException</c> will
	/// execute. If onException is null, the exception will be re-thrown.
	/// </param>
	/// <remarks>Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.</remarks>
	public static async void SafeFireAndForgetAsync<TException>(
		this ValueTask task,
		bool continueOnCapturedContext = true,
		Action<TException>? onException = null)
		where TException : Exception
	{
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext);
		}
		catch (TException ex) when (onException is not null)
		{
			onException.Invoke(ex);
		}
	}
}
