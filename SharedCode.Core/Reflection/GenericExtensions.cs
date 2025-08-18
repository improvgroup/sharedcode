using System.Security;

namespace SharedCode.Reflection;
/// <summary>
/// The generic extension methods class.
/// </summary>
public static class GenericExtensions
{
	/// <summary>
	/// Initializes static members of the <see cref="GenericExtensions" /> class.
	/// </summary>
	/// <exception cref="SecurityException">
	/// DeepCloner should have enough permissions to run. Grant FullTrust or Reflection permission.
	/// </exception>
	[SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
	static GenericExtensions()
	{
		if (!PermissionCheck())
		{
			throw new SecurityException("DeepCloner should have enough permissions to run. Grant FullTrust or Reflection permission.");
		}
	}

	/// <summary>
	/// Performs deep (full) copy of object and related graph.
	/// </summary>
	/// <typeparam name="T">The type of the object being cloned.</typeparam>
	/// <param name="obj">The object being cloned.</param>
	public static T? DeepClone<T>(this T obj) => DeepCloneGenerator.CloneObject(obj);

	/// <summary>
	/// Performs deep (full) copy of object and related graph to existing object.
	/// </summary>
	/// <typeparam name="TFrom">The type of the source object.</typeparam>
	/// <typeparam name="TTo">The type of the target object.</typeparam>
	/// <param name="objFrom">The source object.</param>
	/// <param name="objTo">The target object.</param>
	/// <returns>existing filled object</returns>
	/// <remarks>
	/// Method is valid only for classes, classes should be descendants in reality, not in declaration
	/// </remarks>
	public static TTo? DeepCloneTo<TFrom, TTo>(this TFrom objFrom, TTo objTo) where TTo : class, TFrom => (TTo?)DeepCloneGenerator.CloneObjectTo(objFrom!, objTo, isDeep: true);

	/// <summary>
	/// Performs shallow (only new object returned, without cloning of dependencies) copy of object
	/// </summary>
	/// <typeparam name="T">The type of the object.</typeparam>
	/// <param name="obj">The object.</param>
	public static T? ShallowClone<T>(this T obj) => ShallowCloneGenerator.CloneObject(obj);

	/// <summary>
	/// Performs shallow copy of object to existing object
	/// </summary>
	/// <typeparam name="TFrom">The type of the source object.</typeparam>
	/// <typeparam name="TTo">The type of the target object.</typeparam>
	/// <param name="objFrom">The source object.</param>
	/// <param name="objTo">The target object.</param>
	/// <returns>existing filled object</returns>
	/// <remarks>
	/// Method is valid only for classes, classes should be descendants in reality, not in declaration
	/// </remarks>
	public static TTo? ShallowCloneTo<TFrom, TTo>(this TFrom objFrom, TTo objTo) where TTo : class, TFrom => (TTo?)DeepCloneGenerator.CloneObjectTo(objFrom!, objTo, isDeep: false);

	private static bool PermissionCheck()
	{
		// best way to check required permission: execute something and receive exception .net
		// security policy is weird for normal usage
		try
		{
			_ = new object().ShallowClone();
		}
		catch (VerificationException)
		{
			return false;
		}
		catch (MemberAccessException)
		{
			return false;
		}

		return true;
	}
}
