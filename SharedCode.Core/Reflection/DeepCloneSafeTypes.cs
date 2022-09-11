namespace SharedCode.Reflection;

using SharedCode;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Safe types are types, which can be copied without real cloning. e.g. simple structs or strings
/// </summary>
internal static class DeepCloneSafeTypes
{
	/// <summary>
	/// The known types
	/// </summary>
	internal static readonly ConcurrentDictionary<Type, bool> KnownTypes = new();

	/// <summary>
	/// Initializes static members of the <see cref="DeepCloneSafeTypes" /> class.
	/// </summary>
	static DeepCloneSafeTypes()
	{
		foreach (
			var x in
				new[]
					{
							typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong),
							typeof(float), typeof(double), typeof(decimal), typeof(char), typeof(string), typeof(bool), typeof(DateTime),
							typeof(IntPtr), typeof(UIntPtr), typeof(Guid),
							Type.GetType("System.RuntimeType"),
							Type.GetType("System.RuntimeTypeHandle"),
							StringComparer.Ordinal.GetType(),
							StringComparer.CurrentCulture.GetType(),
					})
		{
			_ = KnownTypes.TryAdd(x!, true);
		}
	}

	/// <summary>
	/// Determines whether this instance [can return same object] the specified type.
	/// </summary>
	/// <param name="type">The type of the object.</param>
	/// <returns>
	/// <c>true</c> if this instance [can return same object] the specified type; otherwise, <c>false</c>.
	/// </returns>
	public static bool CanReturnSameObject(Type type) => CanReturnSameType(type, null);

	[SuppressMessage("Design", "GCop132:Since the type is inferred, use 'var' instead", Justification = "GCop false positive.")]
	private static bool CanReturnSameType(Type type, HashSet<Type>? processingTypes)
	{
		if (KnownTypes.TryGetValue(type, out var isSafe))
		{
			return isSafe;
		}

		// enums are safe pointers (e.g. int*) are unsafe, but we cannot do anything with it except
		// blind copy
		if (type.IsEnum() || type.IsPointer)
		{
			_ = KnownTypes.TryAdd(type, true);
			return true;
		}

		// do not copy db null
		if (type.FullName?.StartsWith("System.DBNull", StringComparison.Ordinal) == true)
		{
			_ = KnownTypes.TryAdd(type, true);
			return true;
		}

		if (type.FullName?.StartsWith("System.RuntimeType", StringComparison.Ordinal) == true)
		{
			_ = KnownTypes.TryAdd(type, true);
			return true;
		}

		if (type.FullName?.StartsWith("System.Reflection.", StringComparison.Ordinal) == true &&
			Equals(type.GetTypeInfo().Assembly, typeof(PropertyInfo).GetTypeInfo().Assembly))
		{
			_ = KnownTypes.TryAdd(type, true);
			return true;
		}

		if (type.IsSubclassOfTypeByName("CriticalFinalizerObject"))
		{
			_ = KnownTypes.TryAdd(type, true);
			return true;
		}

		if (type.FullName?.StartsWith("Microsoft.Extensions.DependencyInjection.", StringComparison.Ordinal) == true)
		{
			_ = KnownTypes.TryAdd(type, true);
			return true;
		}

		if (type.FullName == "Microsoft.EntityFrameworkCore.Internal.ConcurrencyDetector")
		{
			_ = KnownTypes.TryAdd(type, true);
			return true;
		}

		// default comparers should not be cloned due possible comparison
		// EqualityComparer<T>.Default == comparer
		if (type.FullName?.Contains("EqualityComparer", StringComparison.Ordinal) == true)
		{
			if (type.FullName.StartsWith("System.Collections.Generic.GenericEqualityComparer`", StringComparison.Ordinal)
				|| type.FullName.StartsWith("System.Collections.Generic.ObjectEqualityComparer`", StringComparison.Ordinal)
				|| type.FullName.StartsWith("System.Collections.Generic.EnumEqualityComparer`", StringComparison.Ordinal)
				|| type.FullName.StartsWith("System.Collections.Generic.NullableEqualityComparer`", StringComparison.Ordinal)
				|| type.FullName == "System.Collections.Generic.ByteEqualityComparer")
			{
				_ = KnownTypes.TryAdd(type, true);
				return true;
			}
		}

		// classes are always unsafe (we should copy it fully to count references)
		if (!type.IsValueType())
		{
			_ = KnownTypes.TryAdd(type, false);
			return false;
		}

		processingTypes ??= new HashSet<Type>();

		// structs cannot have a loops, but check it anyway
		_ = processingTypes.Add(type);

		List<FieldInfo> fi = new();
		var tp = type;
		do
		{
			fi.AddRange(tp.GetAllFields());
			tp = tp.BaseType();
		}
		while (tp is not null);

		foreach (var fieldInfo in fi)
		{
			var fieldType = fieldInfo.FieldType;
			if (processingTypes.Contains(fieldType))
				continue;

			// not safe and not not safe. we need to go deeper
			if (!CanReturnSameType(fieldType, processingTypes))
			{
				_ = KnownTypes.TryAdd(type, false);
				return false;
			}
		}

		_ = KnownTypes.TryAdd(type, true);
		return true;
	}
}
