// <copyright file="EnumerableExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

#if NETSTANDARD2_0
namespace SharedCode.Collections.Generic;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The enumerable extensions class.
/// </summary>
public static class EnumerableExtensions
{
	/// <summary>
	/// Returns an enumerable sequence of enumerable sequences of the specified size by splitting
	/// this enumerable sequence.
	/// </summary>
	/// <typeparam name="T">The type of items in the input sequence.</typeparam>
	/// <param name="this">The input sequence.</param>
	/// <param name="chunkSize">The size of the chunks.</param>
	/// <returns>
	/// The enumerable sequence of enumerable sequences of the specified size by splitting this
	/// enumerable sequence.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException">chunkSize</exception>
	public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> @this, int chunkSize)
	{
		return chunkSize <= 0
			? throw new ArgumentOutOfRangeException(nameof(chunkSize))
			: ChunkImpl();

		IEnumerable<IEnumerable<T>> ChunkImpl()
		{
			while (@this.Any())
			{
				yield return @this.Take(chunkSize);
				@this = @this.Skip(chunkSize);
			}
		}
	}
}

#endif
