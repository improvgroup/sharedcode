namespace SharedCode.Windows.WPF.ViewModels
{
	using System.Collections.Generic;

	/// <summary>
	/// The navigable interface.
	/// </summary>
	public interface INavigable
	{
		/// <summary>
		/// Gets the route.
		/// </summary>
		/// <value>The route.</value>
		string Route { get; }

		/// <summary>
		/// Gets the route description.
		/// </summary>
		/// <value>The route description.</value>
		string RouteDescription { get; }

		/// <summary>
		/// Childrens the specified prefix.
		/// </summary>
		/// <param name="prefix">The prefix.</param>
		/// <returns>IEnumerable&lt;INavigable&gt;.</returns>
		IEnumerable<INavigable> Children(string prefix);

		/// <summary>
		/// Childs the routes.
		/// </summary>
		/// <param name="prefix">The prefix.</param>
		/// <returns>IEnumerable&lt;System.String&gt;.</returns>
		IEnumerable<string> ChildRoutes(string prefix);

		/// <summary>
		/// Navigates the specified route.
		/// </summary>
		/// <param name="route">The route.</param>
		void Navigate(params string[] route);

		/// <summary>
		/// Uniques the prefixes.
		/// </summary>
		/// <param name="prefix">The prefix.</param>
		/// <returns>IEnumerable&lt;System.String&gt;.</returns>
		IEnumerable<string> UniquePrefixes(string prefix);
	}
}
