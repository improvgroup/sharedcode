// <copyright file="QueryableExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Linq
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// The queryable extensions class.
	/// </summary>
	public static class QueryableExtensions
	{
		/// <summary>
		/// Creates of a <see cref="List{T}" /> from an <see cref="IQueryable{T}" /> asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <param name="source">
		/// The System.Collections.Generic.IEnumerable{T} to create a
		/// System.Collections.Generic.List{T} from.
		/// </param>
		/// <returns>A <see cref="List{T}" /> that contains elements from the input sequence.</returns>
		public static Task<List<T>> ToListAsync<T>(this IQueryable<T> source) => Task.Run(() => source.ToList());
	}
}
