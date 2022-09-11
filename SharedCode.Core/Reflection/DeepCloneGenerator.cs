namespace SharedCode.Reflection;

using System;
using System.Linq;

/// <summary>
/// The deep clone generator class.
/// </summary>
internal static class DeepCloneGenerator
{
	/// <summary>
	/// Clones the object.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;T&gt;.</returns>
	public static T? CloneObject<T>(T obj)
	{
		if (obj is ValueType)
		{
			var type = obj.GetType();
			if (typeof(T) == type)
			{
				return DeepCloneSafeTypes.CanReturnSameObject(type) ? obj : CloneStructInternal(obj, new DeepCloneState());
			}
		}

		return (T?)CloneClassRoot(obj);
	}

	/// <summary>
	/// Clones the values from one object to another.
	/// </summary>
	/// <param name="objFrom">The object from.</param>
	/// <param name="objTo">The object to.</param>
	/// <param name="isDeep">if set to <c>true</c> [is deep].</param>
	/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
	/// <exception cref="ArgumentNullException">objFrom - Cannot copy null object to another</exception>
	/// <exception cref="InvalidOperationException">
	/// From object should be derived from From object, but From object has type " +
	/// objFrom.GetType().FullName + " and to " + objTo.GetType().FullName
	/// </exception>
	/// <exception cref="InvalidOperationException">It is forbidden to clone strings</exception>
	public static object? CloneObjectTo(object objFrom, object? objTo, bool isDeep)
	{
		if (objTo is null)
		{
			return null;
		}

		if (objFrom is null)
		{
			throw new ArgumentNullException(nameof(objFrom), "Cannot copy null object to another");
		}

		var type = objFrom.GetType();
		if (!type.IsInstanceOfType(objTo))
		{
			throw new InvalidOperationException("From object should be derived from From object, but From object has type " + objFrom.GetType().FullName + " and to " + objTo.GetType().FullName);
		}

		if (objFrom is string)
		{
			throw new InvalidOperationException("It is forbidden to clone strings");
		}

		var cloner = (Func<object, object, DeepCloneState, object>)(isDeep
			? DeepCloneCache.GetOrAddDeepClassTo(type, t => ExpressionGenerator.GenerateClonerInternal(t, true))
			: DeepCloneCache.GetOrAddShallowClassTo(type, t => ExpressionGenerator.GenerateClonerInternal(t, false)));
		return cloner is null ? objTo : cloner(objFrom, objTo, new DeepCloneState());
	}

	/// <summary>
	/// Clone1s the dim array class internal.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj">The object.</param>
	/// <param name="state">The state.</param>
	/// <returns>System.Nullable&lt;T&gt;[].</returns>
	internal static T[]? Clone1DimArrayClassInternal<T>(T[]? obj, DeepCloneState state)
	{
		// not null from called method, but will check it anyway
		if (obj is null)
		{
			return null;
		}

		var l = obj.Length;
		var outArray = new T[l];
		state.AddKnownRef(obj, outArray);
		for (var i = 0; i < l; i++)
		{
			outArray[i] = (T)CloneClassInternal(obj[i], state)!;
		}

		return outArray;
	}

	/// <summary>
	/// Clone1s the dim array safe internal.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj">The object.</param>
	/// <param name="state">The state.</param>
	/// <returns>T[].</returns>
	internal static T[] Clone1DimArraySafeInternal<T>(T[] obj, DeepCloneState state)
	{
		var l = obj.Length;
		var outArray = new T[l];
		state.AddKnownRef(obj, outArray);
		Array.Copy(obj, outArray, obj.Length);
		return outArray;
	}

	/// <summary>
	/// Clone1s the dim array structure internal.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj">The object.</param>
	/// <param name="state">The state.</param>
	/// <returns>System.Nullable&lt;T&gt;[].</returns>
	internal static T[]? Clone1DimArrayStructInternal<T>(T[]? obj, DeepCloneState state)
	{
		// not null from called method, but will check it anyway
		if (obj is null)
		{
			return null;
		}

		var l = obj.Length;
		var outArray = new T[l];
		state.AddKnownRef(obj, outArray);
		var cloner = GetClonerForValueType<T>();
		for (var i = 0; i < l; i++)
		{
			outArray[i] = cloner(obj[i], state);
		}

		return outArray;
	}

	/// <summary>
	/// Clone2s the dim array internal.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj">The object.</param>
	/// <param name="state">The state.</param>
	/// <returns>System.Nullable&lt;T&gt;[].</returns>
	[SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "<Pending>")]
	internal static T[,]? Clone2DimArrayInternal<T>(T[,]? obj, DeepCloneState state)
	{
		// not null from called method, but will check it anyway
		if (obj is null)
		{
			return null;
		}

		// we cannot determine by type multidim arrays (one dimension is possible) so, will check
		// for index here
		var lb1 = obj.GetLowerBound(0);
		var lb2 = obj.GetLowerBound(1);
		if (lb1 != 0 || lb2 != 0)
		{
			return (T[,])CloneAbstractArrayInternal(obj, state)!;
		}

		var l1 = obj.GetLength(0);
		var l2 = obj.GetLength(1);
		var outArray = new T[l1, l2];
		state.AddKnownRef(obj, outArray);
		if (DeepCloneSafeTypes.CanReturnSameObject(typeof(T)))
		{
			Array.Copy(obj, outArray, obj.Length);
			return outArray;
		}

		if (typeof(T).IsValueType())
		{
			var cloner = GetClonerForValueType<T>();
			for (var i = 0; i < l1; i++)
			{
				for (var k = 0; k < l2; k++)
				{
					outArray[i, k] = cloner(obj[i, k], state);
				}
			}
		}
		else
		{
			for (var i = 0; i < l1; i++)
			{
				for (var k = 0; k < l2; k++)
				{
					outArray[i, k] = (T)CloneClassInternal(obj[i, k], state)!;
				}
			}
		}

		return outArray;
	}

	/// <summary>
	/// Clones the abstract array internal.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="state">The state.</param>
	/// <returns>System.Nullable&lt;Array&gt;.</returns>
	internal static Array? CloneAbstractArrayInternal(Array? obj, DeepCloneState state)
	{
		// not null from called method, but will check it anyway
		if (obj is null)
		{
			return null;
		}

		var rank = obj.Rank;

		var lengths = Enumerable.Range(0, rank).Select(obj.GetLength).ToArray();

		var lowerBounds = Enumerable.Range(0, rank).Select(obj.GetLowerBound).ToArray();
		var idxes = Enumerable.Range(0, rank).Select(obj.GetLowerBound).ToArray();

		var elementType = obj.GetType().GetElementType();
		var outArray = Array.CreateInstance(elementType!, lengths, lowerBounds);

		state.AddKnownRef(obj, outArray);

		// we're unable to set any value to this array, so, just return it
		if (lengths.Contains(0))
		{
			return outArray;
		}

		if (DeepCloneSafeTypes.CanReturnSameObject(elementType!))
		{
			Array.Copy(obj, outArray, obj.Length);
			return outArray;
		}

		var ofs = rank - 1;
		while (true)
		{
			outArray.SetValue(CloneClassInternal(obj.GetValue(idxes), state), idxes);
			idxes[ofs]++;

			if (idxes[ofs] >= lowerBounds[ofs] + lengths[ofs])
			{
				do
				{
					if (ofs == 0) return outArray;
					idxes[ofs] = lowerBounds[ofs];
					ofs--;
					idxes[ofs]++;
				} while (idxes[ofs] >= lowerBounds[ofs] + lengths[ofs]);

				ofs = rank - 1;
			}
		}
	}

	/// <summary>
	/// Clones the class internal.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="state">The state.</param>
	/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
	internal static object? CloneClassInternal(object? obj, DeepCloneState state)
	{
		if (obj is null)
		{
			return null;
		}

		var cloner = (Func<object, DeepCloneState, object>?)DeepCloneCache.GetOrAddClass(obj.GetType(), t => GenerateCloner(t, true));

		if (cloner is null)
		{
			return obj;
		}

		// loop
		var knownRef = state.GetKnownRef(obj);
		return knownRef ?? cloner(obj, state);
	}

	/// <summary>
	/// Gets the type of the cloner for value.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>Func&lt;T, DeepCloneState, T&gt;.</returns>
	internal static Func<T, DeepCloneState, T> GetClonerForValueType<T>() =>
		(Func<T, DeepCloneState, T>)DeepCloneCache.GetOrAddStructAsObject(typeof(T), t => GenerateCloner(t, false));

	private static object? CloneClassRoot(object? obj)
	{
		if (obj is null)
		{
			return null;
		}

		var cloner = (Func<object, DeepCloneState, object>?)DeepCloneCache.GetOrAddClass(obj.GetType(), t => GenerateCloner(t, true));
		return cloner is null ? obj : cloner(obj, new DeepCloneState());
	}

	private static T CloneStructInternal<T>(T obj, DeepCloneState state)
	{
		// no loops, no nulls, no inheritance
		var cloner = GetClonerForValueType<T>();

		// safe ojbect
		return cloner is null ? obj : cloner(obj, state);
	}

	private static object? GenerateCloner(Type t, bool asObject)
	{
		return DeepCloneSafeTypes.CanReturnSameObject(t) && asObject && !t.IsValueType()
			? null
			: DeepCloneExpressionGenerator.GenerateClonerInternal(t, asObject);
	}
}
