namespace SharedCode.Windows.WPF.ViewModels;

/// <summary>
/// The window interface.
/// </summary>
public interface IWindow : ICloseable
{
	/// <summary>
	/// Closes the application.
	/// </summary>
	void CloseApplication();

	/// <summary>
	/// Minimizes the application.
	/// </summary>
	void MinimizeApplication();

	/// <summary>
	/// Sets the title.
	/// </summary>
	/// <param name="title">The window title.</param>
	void SetTitle(string title);
}
