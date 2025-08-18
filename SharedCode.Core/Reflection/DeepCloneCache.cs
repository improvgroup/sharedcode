using System.Collections.Concurrent;

namespace SharedCode.Reflection;
internal static class DeepCloneCache
{
	private static readonly ConcurrentDictionary<Type, object> _structAsObjectCache = new();
	private static readonly ConcurrentDictionary<Type, object> _typeCache = new();
	private static readonly ConcurrentDictionary<Type, object> _typeCacheDeepTo = new();
	private static readonly ConcurrentDictionary<Type, object> _typeCacheShallowTo = new();
	private static readonly ConcurrentDictionary<Tuple<Type, Type>, object> _typeConvertCache = new();

	/// <summary>
	/// This method can be used when we switch between safe / unsafe variants (for testing)
	/// </summary>
	public static void ClearCache()
	{
		_typeCache.Clear();
		_typeCacheDeepTo.Clear();
		_typeCacheShallowTo.Clear();
		_structAsObjectCache.Clear();
		_typeConvertCache.Clear();
	}

	/// <summary>
	/// Gets the or add class.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="type">The type.</param>
	/// <param name="adder">The adder.</param>
	/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
	[SuppressMessage("Reliability", "CA2002:Do not lock on objects with weak identity", Justification = "<Pending>")]
	public static object? GetOrAddClass<T>(Type type, Func<Type, T> adder)
	{
		// this implementation is slightly faster than getoradd
		if (_typeCache.TryGetValue(type, out var value))
		{
			return value;
		}

		// will lock by type object to ensure only one type generator is generated simultaneously
		lock (type)
		{
			value = _typeCache.GetOrAdd(type, t => adder(t)!);
		}

		return value;
	}

	/// <summary>
	/// Gets the or add convertor.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="adder">The adder.</param>
	/// <returns>T.</returns>
	public static T GetOrAddConvertor<T>(Type from, Type to, Func<Type, Type, T> adder) =>
		(T)_typeConvertCache.GetOrAdd(new Tuple<Type, Type>(from, to), (tuple) => adder(tuple.Item1, tuple.Item2)!);

	/// <summary>
	/// Gets the or add deep class to.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="type">The type.</param>
	/// <param name="adder">The adder.</param>
	/// <returns>System.Object.</returns>
	[SuppressMessage("Reliability", "CA2002:Do not lock on objects with weak identity", Justification = "<Pending>")]
	public static object GetOrAddDeepClassTo<T>(Type type, Func<Type, T> adder)
	{
		if (_typeCacheDeepTo.TryGetValue(type, out var value))
		{
			return value;
		}

		// will lock by type object to ensure only one type generator is generated simultaneously
		lock (type)
		{
			value = _typeCacheDeepTo.GetOrAdd(type, t => adder(t)!);
		}

		return value;
	}

	/// <summary>
	/// Gets the or add shallow class to.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="type">The type.</param>
	/// <param name="adder">The adder.</param>
	/// <returns>System.Object.</returns>
	[SuppressMessage("Reliability", "CA2002:Do not lock on objects with weak identity", Justification = "<Pending>")]
	public static object GetOrAddShallowClassTo<T>(Type type, Func<Type, T> adder)
	{
		if (_typeCacheShallowTo.TryGetValue(type, out var value))
		{
			return value;
		}

		// will lock by type object to ensure only one type generator is generated simultaneously
		lock (type)
		{
			value = _typeCacheShallowTo.GetOrAdd(type, t => adder(t)!);
		}

		return value;
	}

	/// <summary>
	/// Gets the or add structure as object.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="type">The type.</param>
	/// <param name="adder">The adder.</param>
	/// <returns>System.Object.</returns>
	[SuppressMessage("Reliability", "CA2002:Do not lock on objects with weak identity", Justification = "<Pending>")]
	public static object GetOrAddStructAsObject<T>(Type type, Func<Type, T> adder)
	{
		// this implementation is slightly faster than getoradd
		if (_structAsObjectCache.TryGetValue(type, out var value))
		{
			return value;
		}

		// will lock by type object to ensure only one type generator is generated simultaneously
		lock (type)
		{
			value = _structAsObjectCache.GetOrAdd(type, t => adder(t)!);
		}

		return value;
	}
}
