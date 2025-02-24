
namespace SharedCode.Collections;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;

/// <summary>
/// The array extensions class
/// </summary>
public static class ArrayExtensions
{
	/// <summary>
	/// Converts an Array of arbitrary type to an array of type T. If a suitable converter cannot be
	/// found to do the conversion, a NotSupportedException is thrown.
	/// </summary>
	/// <typeparam name="T">The type of items in the output array.</typeparam>
	/// <param name="this">The input array.</param>
	/// <returns>The new array.</returns>
	/// <exception cref="NotSupportedException">
	/// A suitable converter cannot be found to do the conversion.
	/// </exception>
	/// <exception cref="ArgumentNullException">input</exception>
	public static T?[] ConvertTo<T>(this Array @this)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		Contract.Ensures(Contract.Result<T[]>() is not null);

		var result = new T?[@this.Length];
		var tc = TypeDescriptor.GetConverter(typeof(T));
		var type = @this.GetValue(0)?.GetType() ?? @this.GetType().GetElementType() ?? typeof(object);
		if (tc.CanConvertFrom(type))
		{
			for (var i = 0; i < @this.Length; i++)
			{
				var value = @this.GetValue(i);
				result[i] = (T?)(value is null ? null : tc.ConvertFrom(value));
			}
		}
		else
		{
			tc = TypeDescriptor.GetConverter(type);
			if (tc.CanConvertTo(typeof(T)))
			{
				for (var i = 0; i < @this.Length; i++)
				{
					result[i] = (T?)tc.ConvertTo(@this.GetValue(i), typeof(T));
				}
			}
			else
			{
				throw new NotSupportedException("A suitable converter cannot be found to do the conversion.");
			}
		}

		return result;
	}

	/// <summary>
	/// Converts an array of any type to <see cref="List{T}" /> passing a mapping delegate
	/// Func{object, T} that returns type T. If T is null, it will not be added to the collection.
	/// If the array is null, then a new instance of <see cref="List{T}" /> is returned.
	/// </summary>
	/// <typeparam name="T">The type of the items in the output list.</typeparam>
	/// <param name="this">The array of items.</param>
	/// <param name="mapFunction">The map function.</param>
	/// <returns>The output list.</returns>
	public static IList<T> ToList<T>(this Array @this, Func<object, T> mapFunction)
	{
		Contract.Ensures(Contract.Result<List<T>>() is not null);

		if (@this is null || mapFunction is null)
		{
			return new List<T>();
		}

		var coll = new List<T>();
		for (var i = 0; i < @this.Length; i++)
		{
			var handler = mapFunction;
			if (handler is not null)
			{
				var arg = @this.GetValue(i);
				var val = arg is null ? default : handler(arg);
				if (val is not null)
				{
					coll.Add(val);
				}
			}
		}

		return coll;
	}
}
