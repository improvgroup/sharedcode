namespace SharedCode.Windows.WPF.ViewModels
{
	using SharedCode.Linq;
	using SharedCode.Windows.WPF;

	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	/// <summary>
	/// An abstract view model class.
	/// </summary>
	public abstract class ViewModelBase : BindableBase
	{
		/// <summary>
		/// Runs a command if the updating flag is not set.
		/// If the flag is true, indicating the function is already running, then the action is not run.
		/// If the flag is false, indicating the function is not running, then the action is run.
		/// Once the action is finished, if it was run, then the flag is reset to false.
		/// </summary>
		/// <param name="updatingFlag">The boolean value indicating whether the function is running.</param>
		/// <param name="action">The action to run, if it is not already running.</param>
		/// <returns>A <see cref="Task"/>.</returns>
		protected static async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
		{
			_ = action ?? throw new ArgumentNullException(nameof(action));

			if (updatingFlag is not null)
			{
				if (updatingFlag.GetPropertyValue())
					return;

				updatingFlag.SetPropertyValue(true);

				try
				{
					await action().ConfigureAwait(true);
				}
				finally
				{
					updatingFlag.SetPropertyValue(false);
				}
			}
		}
	}
}
