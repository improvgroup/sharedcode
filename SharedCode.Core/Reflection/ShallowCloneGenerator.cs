namespace SharedCode.Reflection;
internal static class ShallowCloneGenerator
{
	/// <summary>
	/// Clones the object.
	/// </summary>
	/// <typeparam name="T">The type of the object.</typeparam>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;T&gt;.</returns>
	public static T? CloneObject<T>(T obj) =>
		obj switch
		{
			ValueType => typeof(T) == obj.GetType()
				? obj
				: (T)ShallowCloneHelper.CloneObject(obj),
			null => (T?)(object?)null,
			_ => DeepCloneSafeTypes.CanReturnSameObject(obj.GetType())
				? obj
				: (T)ShallowCloneHelper.CloneObject(obj)
		};
}
