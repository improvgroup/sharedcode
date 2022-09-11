namespace SharedCode.Reflection;

using SharedCode;

/// <summary>
/// The shallow object clone helper class.
/// </summary>
public abstract class ShallowCloneHelper
{
	private static ShallowCloneHelper _instance = new ShallowSafeObjectCloneHelper();

	/// <summary>
	/// The unsafe instance
	/// </summary>
	/// <remarks>no unsafe variant for core</remarks>
	private static readonly ShallowCloneHelper _unsafeInstance = _instance;

	/// <summary>
	/// Performs a shallow object clone.
	/// </summary>
	/// <param name="obj">The object to clone.</param>
	public static object CloneObject(object obj) => _instance.DoCloneObject(obj);

	/// <summary>
	/// Determines whether [is safe variant].
	/// </summary>
	/// <returns><c>true</c> if [is safe variant]; otherwise, <c>false</c>.</returns>
	internal static bool IsSafeVariant() => _instance is ShallowSafeObjectCloneHelper;

	/// <summary>
	/// Purpose of this method is testing variants
	/// </summary>
	/// <param name="isSafe">A value indicating whether to use the safe clone helper.</param>
	internal static void SwitchTo(bool isSafe)
	{
		DeepCloneCache.ClearCache();
		_instance = isSafe ? new ShallowSafeObjectCloneHelper() : _unsafeInstance;
	}

	/// <summary>
	/// Does the actual object cloning.
	/// </summary>
	/// <param name="obj">The object to clone.</param>
	protected abstract object DoCloneObject(object obj);

	private class ShallowSafeObjectCloneHelper : ShallowCloneHelper
	{
		private static readonly Func<object, object> _cloneFunc =
			Expression
				.Lambda<Func<object, object>>(
					Expression.Call(
						Expression.Parameter(typeof(object)),
						typeof(object).GetPrivateMethod(nameof(MemberwiseClone))!),
					Expression.Parameter(typeof(object)))
				.Compile();

		/// <summary>
		/// Does the actual object cloning.
		/// </summary>
		/// <param name="obj">The object to clone.</param>
		/// <returns>System.Object.</returns>
		protected override object DoCloneObject(object obj) => _cloneFunc(obj);
	}
}
