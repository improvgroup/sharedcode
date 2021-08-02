namespace SharedCode.Windows.WPF.Commands
{
	using System.Threading.Tasks;
	using System.Windows.Input;

	/// <summary>
	/// Implements the <see cref="ICommand" /> asynchronously.
	/// </summary>
	/// <seealso cref="ICommand" />
	public interface IAsyncCommand : ICommand
	{
		/// <summary>
		/// Executes the <see cref="ICommand"/> as a <see cref="Task"/>.
		/// </summary>
		/// <returns>The executed <see cref="Task"/>.</returns>
		Task ExecuteAsync();
	}
}
