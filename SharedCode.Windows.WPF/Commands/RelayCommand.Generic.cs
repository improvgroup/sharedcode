namespace SharedCode.Windows.WPF.Commands
{
	using System;
	using System.Globalization;
	using System.Windows.Input;

	/// <summary>
	/// Class RelayCommand. Implements the <see cref="ICommand" />.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="ICommand" />
	public class RelayCommand<T> : ICommand
	{
		private readonly Predicate<T>? canExecute;
		private readonly Action<T> execute;

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
		/// </summary>
		/// <param name="execute">The execute.</param>
		public RelayCommand(Action<T> execute)
			: this(execute, null) =>
			this.execute = execute;

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
		/// </summary>
		/// <param name="execute">The execute.</param>
		/// <param name="canExecute">The can execute.</param>
		/// <exception cref="ArgumentNullException">execute</exception>
		public RelayCommand(Action<T> execute, Predicate<T>? canExecute)
		{
			this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
			this.canExecute = canExecute;
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command. If the command does not require data to be passed, this object
		/// can be set to <see langword="null" />.
		/// </param>
		/// <returns>
		/// <see langword="true" /> if this command can be executed; otherwise, <see
		/// langword="false" />.
		/// </returns>
		public bool CanExecute(object? parameter)
		{
			if (this.canExecute is null)
			{
				return true;
			}
			else if (parameter is T param)
			{
				return this.canExecute(param);
			}
			else
			{
#pragma warning disable CS8604 // Possible null reference argument.
				return this.canExecute(default);
#pragma warning restore CS8604 // Possible null reference argument.
			}
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command. If the command does not require data to be passed, this object
		/// can be set to <see langword="null" />.
		/// </param>
		public void Execute(object? parameter)
		{
			if (parameter is T param)
			{
				this.execute((T)parameter);
			}
			else
			{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
				T value;
				try
				{
					value = (T)Convert.ChangeType(parameter, typeof(T), CultureInfo.CurrentCulture);
				}
				catch (InvalidCastException)
				{
					value = default;
				}

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
				this.execute(value);
#pragma warning restore CS8604 // Possible null reference argument.
			}
		}

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
		public event EventHandler? CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}
}
