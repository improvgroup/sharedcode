namespace SharedCode.Windows.WPF.Commands
{
	using System.Threading.Tasks;
	using System.Windows.Input;

	/// <summary>
	/// Implements the <see cref="ICommand" /> asynchronously.
	/// </summary>
	/// <typeparam name="T">The type of the argument of the command.</typeparam>
	/// <seealso cref="ICommand" />
	public interface IAsyncCommand<T> : ICommand
	{
		/// <summary>
		/// Executes the <see cref="ICommand" /> as a <see cref="Task" />.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command. If the command does not require data to be passed, this object
		/// can be set to null.
		/// </param>
		/// <returns>The executed Task</returns>
		Task ExecuteAsync(T parameter);
	}
}
