namespace SharedCode.Windows.WPF.ViewModels
{
	/// <summary>
	/// The window interface.
	/// </summary>
	public interface IWindow
	{
		/// <summary>
		/// Closes the window.
		/// </summary>
		void Close();

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
		/// <param name="title">The title.</param>
		void SetTitle(string title);
	}
}
