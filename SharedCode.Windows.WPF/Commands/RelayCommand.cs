namespace SharedCode.Windows.WPF.Commands
{
	using System;
	using System.Windows.Input;

	public class RelayCommand : ICommand
	{
		private readonly Predicate<object?>? canExecute;
		private readonly Action<object?> execute;

		public RelayCommand(Action<object?> execute)
			: this(execute, null) =>
			this.execute = execute;

		public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
		{
			this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
			this.canExecute = canExecute;
		}

		/// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
		/// <returns>
		///   <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.</returns>
		public bool CanExecute(object? parameter) => this.canExecute is null || this.canExecute(parameter);

		/// <summary>Defines the method to be called when the command is invoked.</summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
		public void Execute(object? parameter) => this.execute(parameter);

		/// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
		/// <remarks>
		/// Ensures WPF commanding infrastructure asks all RelayCommand objects whether their
		/// associated views should be enabled whenever a command is invoked
		/// </remarks>
		public event EventHandler? CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
				CanExecuteChangedInternal += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
				CanExecuteChangedInternal -= value;
			}
		}

		private event EventHandler? CanExecuteChangedInternal;


		/// <summary>
		/// Raises the <see cref="CanExecuteChanged" /> event.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "<Pending>")]
		public void RaiseCanExecuteChanged() => CanExecuteChangedInternal?.Invoke(this, EventArgs.Empty);
	}
}
