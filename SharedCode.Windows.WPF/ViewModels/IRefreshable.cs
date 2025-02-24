
namespace SharedCode.Windows.WPF.ViewModels
{
	using System.Windows.Input;

	/// <summary>
	/// The refreshable interface.
	/// </summary>
	public interface IRefreshable
	{
		/// <summary>
		/// Gets the refresh command.
		/// </summary>
		/// <value>The refresh command.</value>
		ICommand RefreshCommand { get; }
	}
}
