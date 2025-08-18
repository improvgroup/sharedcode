namespace SharedCode.Windows.WPF.ViewModels;

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
	/// Returns the children with the specified prefix.
	/// </summary>
	/// <param name="prefix">The child prefix.</param>
	/// <returns>IEnumerable&lt;INavigable&gt;.</returns>
	IEnumerable<INavigable> Children(string prefix);

	/// <summary>
	/// Returns the routes of the children with the specified prefix.
	/// </summary>
	/// <param name="prefix">The child prefix.</param>
	/// <returns>IEnumerable&lt;System.String&gt;.</returns>
	IEnumerable<string> ChildRoutes(string prefix);

	/// <summary>
	/// Navigates to the specified route.
	/// </summary>
	/// <param name="route">The route to navigate to.</param>
	void Navigate(params string[] route);

	/// <summary>
	/// Returns the unique prefixes.
	/// </summary>
	/// <param name="prefix">The root prefix.</param>
	/// <returns>IEnumerable&lt;System.String&gt;.</returns>
	IEnumerable<string> UniquePrefixes(string prefix);
}
