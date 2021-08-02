namespace SharedCode.Windows.WPF.Commands
{
	using SharedCode.Threading.Tasks;

	using System;
	using System.Threading.Tasks;
	using System.Windows.Input;

	/// <summary>
	/// Allows Commands to safely be used asynchronously with <see cref="Task"/>. Implements the <see cref="IAsyncCommand{T}" />.
	/// </summary>
	/// <typeparam name="T">The type of the command argument.</typeparam>
	/// <seealso cref="IAsyncCommand{T}" />
	public sealed class AsyncCommand<T> : IAsyncCommand<T>
	{
		/// <summary>
		/// The can execute
		/// </summary>
		private readonly Func<object?, bool> canExecute;

		/// <summary>
		/// The continue on captured context
		/// </summary>
		private readonly bool continueOnCapturedContext;

		/// <summary>
		/// The execute
		/// </summary>
		private readonly Func<T, Task> execute;

		/// <summary>
		/// The on exception
		/// </summary>
		private readonly Action<Exception>? onException;

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncCommand{T}" /> class.
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAysnc is called. This does not check
		/// canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException
		/// is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the
		/// Synchronization Context returns to the calling thread. If set to <c>false</c> continue
		/// on a different context; this will allow the Synchronization Context to continue on a
		/// different thread</param>
		/// <exception cref="ArgumentNullException">execute</exception>
		public AsyncCommand(
			Func<T, Task> execute,
			Func<object?, bool>? canExecute = null,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = true)
		{
			this.execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
			this.canExecute = canExecute ?? (_ => true);
			this.onException = onException;
			this.continueOnCapturedContext = continueOnCapturedContext;
		}

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute
		/// </summary>
		public event EventHandler? CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		/// <inheritdoc />
		public bool CanExecute(object? parameter) => this.canExecute(parameter);


		/// <inheritdoc />
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Concurrency", "PH_S030:Async Void Method Invocation", Justification = "<Pending>")]
		public void Execute(object? parameter)
		{
			if (parameter is T validParameter)
				this.ExecuteAsync(validParameter).SafeFireAndForgetAsync(this.continueOnCapturedContext, this.onException);
			else if (!typeof(T).IsValueType && parameter is null)
			{
#pragma warning disable CS8604 // Possible null reference argument.
				this.ExecuteAsync(default).SafeFireAndForgetAsync(this.continueOnCapturedContext, this.onException);
#pragma warning restore CS8604 // Possible null reference argument.
			}
			else
			{
				throw new InvalidCommandParameterException(typeof(T), parameter?.GetType());
			}
		}

		/// <inheritdoc />
		public Task ExecuteAsync(T parameter) => this.execute(parameter);
	}
}
