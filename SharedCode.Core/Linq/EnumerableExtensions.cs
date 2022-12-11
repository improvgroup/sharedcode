// <copyright file="EnumerableExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Linq
{
	using Collections;

	using SharedCode.Linq;

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Diagnostics.Contracts;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;
	using System.Text;

	/// <summary>
	/// The enumerable extensions class.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Gets the random.
		/// </summary>
		/// <value>The random.</value>
		private static readonly Random Random = new();

		/// <summary>
		/// Aggregates the source.
		/// </summary>
		/// <typeparam name="T">The type of the items in the source.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <param name="aggregateFunction">The aggregate function.</param>
		/// <returns>The result.</returns>
		public static T? Aggregate<T>(this IEnumerable<T> source, Func<T?, T?, T?> aggregateFunction) => source.Aggregate(default, aggregateFunction);

		/// <summary>
		/// Aggregates the source.
		/// </summary>
		/// <typeparam name="T">The type of the items in the source.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <param name="aggregateFunction">The aggregate function.</param>
		/// <returns>The result.</returns>
		public static T? Aggregate<T>(this IEnumerable<T> source, T? defaultValue, Func<T?, T?, T?> aggregateFunction) =>
			source?.Any() ?? false ? source.Aggregate(aggregateFunction) : defaultValue;

		/// <summary>
		/// Starts execution of IQueryable on a ThreadPool thread and returns immediately with a
		/// "end" method to call once the result is needed.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <param name="asyncSelector">The asynchronous selector.</param>
		/// <returns>Func&lt;TResult&gt;.</returns>
		/// <exception cref="ArgumentNullException">source or asyncSelector</exception>
		[SuppressMessage("Roslynator", "RCS1047:Non-asynchronous method name should not end with 'Async'.", Justification = "<Pending>")]
		public static Func<TResult> Async<T, TResult>(this IEnumerable<T> source, Func<IEnumerable<T>, TResult> asyncSelector)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));
			_ = asyncSelector ?? throw new ArgumentNullException(nameof(asyncSelector));
			Contract.Ensures(Contract.Result<Func<TResult>>() is not null);
			Debug.Assert(source is not ICollection, "Async does not work on arrays/lists/collections, only on true enumerables/queryables.");

			// Create delegate to exec async
			var work = asyncSelector;

			// Launch it
			var result = work.BeginInvoke(source, null, null);

			// Return method that will block until completed and rethrow exceptions if any
			return () => work.EndInvoke(result);
		}

		/// <summary>
		/// Returns a lazy evaluated enumerable.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <returns>The results.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static IEnumerable<T> Cache<T>(this IEnumerable<T> source)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));

			return CacheHelper(source.GetEnumerator());
		}

		/// <summary>
		/// Returns all combinations of a chosen amount of selected elements in the sequence.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequence.</typeparam>
		/// <param name="source">The source for this extension method.</param>
		/// <param name="select">The amount of elements to select for every combination.</param>
		/// <param name="repetition">True when repetition of elements is allowed.</param>
		/// <returns>All combinations of a chosen amount of selected elements in the sequence.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int select, bool repetition = false)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));
			Contract.Requires(select >= 0);
			Contract.Ensures(Contract.Result<IEnumerable<IEnumerable<T>>>() is not null);

			return select == 0
				? (IEnumerable<IEnumerable<T>>)(new[] { Array.Empty<T>() })
				: source.SelectMany(
					(element, index) =>
						source
						.Skip(repetition ? index : index + 1)
						.Combinations(select - 1, repetition)
						.Select(c => new[] { element }.Concat(c)));
		}

		/// <summary>
		/// Provides a Distinct method that takes a key selector lambda as parameter. The .NET
		/// framework only provides a Distinct method that takes an instance of an implementation of
		/// <see cref="IEqualityComparer{T}" /> where the standard parameterless Distinct that uses
		/// the default equality comparer doesn't suffice.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <param name="keySelector">The key selector.</param>
		/// <returns>The enumerable.</returns>
		public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) =>
			source
				?.GroupBy(keySelector)
				.Select(e => e.First())
				?? Enumerable.Empty<T>();

		/// <summary>
		/// For each item in the enumerable performs the specified action.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <param name="action">The action to be performed on each item.</param>
		/// <returns>The enumerable.</returns>
		/// <exception cref="ArgumentNullException">source or action</exception>
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));
			_ = action ?? throw new ArgumentNullException(nameof(action));

			return source.Select(
				item =>
				{
					action(item);
					return item;
				});
		}

		/// <summary>
		/// For each item in the enumerable performs the specified action.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <param name="action">The action to be performed on each item.</param>
		/// <returns>The enumerable.</returns>
		/// <exception cref="ArgumentNullException">source or action</exception>
		public static IEnumerable<T> ForEach<T>(this IEnumerable source, Action<T> action)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));
			_ = action ?? throw new ArgumentNullException(nameof(action));

			return source.Cast<T>().ForEach(action);
		}

		/// <summary>
		/// For each item in the enumerable performs the specified action.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <typeparam name="TResult">The type of the items in the result.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <param name="function">The function to be executed on each item.</param>
		/// <returns>The enumerable.</returns>
		/// <exception cref="ArgumentNullException">source or function</exception>
		public static IEnumerable<TResult> ForEach<T, TResult>(this IEnumerable<T> source, Func<T, TResult> function)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));
			_ = function ?? throw new ArgumentNullException(nameof(function));

			return source.Select(function);
		}

		/// <summary>
		/// Returns the index of the first occurrence in a sequence by using the default equality comparer.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence in which to locate a value.</param>
		/// <param name="value">The object to locate in the sequence</param>
		/// <returns>
		/// The zero-based index of the first occurrence of value within the entire sequence, if
		/// found; otherwise, –1.
		/// </returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource value) where TSource : IEquatable<TSource>
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));

			return source.IndexOf(value, EqualityComparer<TSource>.Default);
		}

		/// <summary>
		/// Returns the index of the first occurrence in a sequence by using a specified IEqualityComparer.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence in which to locate a value.</param>
		/// <param name="value">The object to locate in the sequence</param>
		/// <param name="comparer">An equality comparer to compare values.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of value within the entire sequence, if
		/// found; otherwise, –1.
		/// </returns>
		/// <exception cref="ArgumentNullException">comparer</exception>
		public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
		{
			_ = comparer ?? throw new ArgumentNullException(nameof(comparer));

			if (source is null)
			{
				return -1;
			}

			var index = 0;
			foreach (var item in source)
			{
				if (comparer.Equals(item, value))
				{
					return index;
				}

				index++;
			}

			return -1;
		}

		/// <summary>
		/// Determines whether the source enumerable is not null and contains items.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <returns>
		/// <c>true</c> if the source enumerable is not null and contains items; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source) => source?.Any() == true;

		/// <summary>
		/// Determines whether the source enumerable is null or contains no items.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <returns>
		/// <c>true</c> if the source enumerable is null or contains no items; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => !source.IsNotNullOrEmpty();

		/// <summary>
		/// Sorts the specified enumerable by the sort expression.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <param name="sortExpression">The sort expression.</param>
		/// <returns>The sorted enumerable.</returns>
		/// <exception cref="Exception">No property x in type T.</exception>
		[SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
		public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string sortExpression)
		{
			sortExpression += string.Empty;
			var parts = sortExpression.Split(' ');
			var descending = false;

			if (parts.Length == 0 || string.IsNullOrEmpty(parts[0]))
			{
				return source;
			}

			var property = parts[0];

			if (parts.Length > 1)
			{
				descending = parts[1].Contains("esc");
			}

			var prop = typeof(T).GetProperty(property);

			return prop is null
				? throw new Exception($"No property '{property}' in {typeof(T).Name}")
				: (IEnumerable<T>)(descending
					? source.OrderByDescending(x => prop.GetValue(x, null))
					: source.OrderBy(x => prop.GetValue(x, null)));
		}

		/// <summary>
		/// Sorts the specified enumerable by the key selector function in the direction specified.
		/// </summary>
		/// <typeparam name="TSource">The type of the source enumerable.</typeparam>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <param name="enumerable">The source enumerable.</param>
		/// <param name="keySelector">The key selector.</param>
		/// <param name="descending">if set to <c>true</c> the sort direction is descending.</param>
		/// <returns>The sorted enumerable.</returns>
		/// <exception cref="ArgumentNullException">keySelector</exception>
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, bool descending)
		{
			_ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

			return (descending ? enumerable?.OrderByDescending(keySelector) : enumerable?.OrderBy(keySelector)) ?? Enumerable.Empty<TSource>().OrderBy(i => i);
		}

		/// <summary>
		/// Sorts the specified enumerable by the key selector function in the direction specified.
		/// </summary>
		/// <typeparam name="TSource">The type of the source enumerable.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <param name="keySelector1">The key first selector.</param>
		/// <param name="keySelector2">The key second selector.</param>
		/// <param name="keySelectors">The remaining key selectors.</param>
		/// <returns>The sorted enumerable.</returns>
		/// <exception cref="ArgumentNullException">
		/// The <paramref name="source">source</paramref> or <paramref
		/// name="keySelectors">keySelector</paramref> is null.
		/// </exception>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="int.MaxValue"></see> elements.
		/// </exception>
		public static IOrderedEnumerable<TSource> OrderBy<TSource>(
			this IEnumerable<TSource> source,
			Func<TSource, IComparable> keySelector1,
			Func<TSource, IComparable> keySelector2,
			params Func<TSource, IComparable>[] keySelectors)
		{
			_ = keySelector1 ?? throw new ArgumentNullException(nameof(keySelector1));
			_ = keySelector2 ?? throw new ArgumentNullException(nameof(keySelector2));

			if (source is null)
			{
				return Enumerable.Empty<TSource>().OrderBy(i => i);
			}

			var current = source;

			if (keySelectors is not null)
			{
				for (var i = keySelectors.Length - 1; i >= 0; i--)
				{
					current = current.OrderBy(keySelectors[i]);
				}
			}

			current = current.OrderBy(keySelector2);

			return current.OrderBy(keySelector1);
		}

		/// <summary>
		/// Sorts the specified enumerable by the key selector function in the direction specified.
		/// </summary>
		/// <typeparam name="TSource">The type of the source enumerable.</typeparam>
		/// <param name="enumerable">The source enumerable.</param>
		/// <param name="keySelector">The key selector.</param>
		/// <param name="descending">if set to <c>true</c> the sort direction is descending.</param>
		/// <param name="keySelectors">The remaining key selectors.</param>
		/// <returns>The sorted enumerable.</returns>
		/// <exception cref="ArgumentNullException">keySelector</exception>
		public static IOrderedEnumerable<TSource> OrderBy<TSource>(
			this IEnumerable<TSource> enumerable,
			Func<TSource, IComparable> keySelector,
			bool descending,
			params Func<TSource, IComparable>[] keySelectors)
		{
			_ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

			if (enumerable is null)
			{
				return Enumerable.Empty<TSource>().OrderBy(i => i);
			}

			var current = enumerable;

			if (keySelectors is not null)
			{
				for (var i = keySelectors.Length - 1; i >= 0; i--)
				{
					current = current.OrderBy(keySelectors[i], descending);
				}
			}

			return current.OrderBy(keySelector, descending);
		}

		/// <summary>
		/// Groups the elements of a sequence according to a specified firstKey selector function
		/// and rotates the unique values from the secondKey selector function into multiple values
		/// in the output, and performs aggregations.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <typeparam name="TFirstKey">The type of the first key.</typeparam>
		/// <typeparam name="TSecondKey">The type of the second key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <param name="firstKeySelector">The first key selector.</param>
		/// <param name="secondKeySelector">The second key selector.</param>
		/// <param name="aggregate">The aggregate function.</param>
		/// <returns>Dictionary&lt;TFirstKey, Dictionary&lt;TSecondKey, TValue&gt;&gt;.</returns>
		/// <exception cref="Exception">A delegate callback throws an exception.</exception>
		/// <exception cref="ArgumentNullException">source or firstKeySelector or secondKeySelector</exception>
		public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(
			this IEnumerable<TSource> source,
			Func<TSource, TFirstKey> firstKeySelector,
			Func<TSource, TSecondKey> secondKeySelector,
			Func<IEnumerable<TSource>, TValue> aggregate)
			where TFirstKey : notnull
			where TSecondKey : notnull
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));
			_ = firstKeySelector ?? throw new ArgumentNullException(nameof(firstKeySelector));
			_ = secondKeySelector ?? throw new ArgumentNullException(nameof(secondKeySelector));
			Contract.Ensures(Contract.Result<Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>>() is not null);

			var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

			foreach (var item in source.ToLookup(firstKeySelector))
			{
				var dict = new Dictionary<TSecondKey, TValue>();
				retVal.Add(item.Key, dict);
				foreach (var subitem in item.ToLookup(secondKeySelector))
				{
					var handler = aggregate;
					if (handler is not null)
					{
						dict.Add(subitem.Key, handler(subitem));
					}
				}
			}

			return retVal;
		}

		/// <summary>
		/// Randomizes order of the items in the specified enumerable.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <returns>The enumerable with random order applied.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="source">source</paramref> is null.
		/// </exception>
		[SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "<Pending>")]
		public static IOrderedEnumerable<T> Randomize<T>(this IEnumerable<T> source) => (source ?? Enumerable.Empty<T>()).OrderBy(_ => Random.Next());

		/// <summary>
		/// This method selects a random element from an Enumerable with only one pass (O(N)
		/// complexity). It contains optimizations for argumens that implement ICollection&lt;T&gt;
		/// by using the Count property and the ElementAt LINQ method. The ElementAt LINQ method
		/// itself contains optimizations for IList&lt;T&gt;
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <returns>A randomly selected item from the enumerable.</returns>
		[SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "<Pending>")]
		public static T? SelectRandom<T>(this IEnumerable<T> source)
		{
			if (source is null)
			{
				return default;
			}

			if (!source.Any())
			{
				return default;
			}

			if (source is ICollection<T> collection)
			{
				return collection.ElementAt(Random.Next(collection.Count));
			}

			var count = 1;
			var selected = default(T);

			foreach (var element in source)
			{
				if (Random.Next(count++) == 0)
				{
					// Select the current element with 1/count probability
					selected = element;
				}
			}

			return selected;
		}

		/// <summary>
		/// Slices the source from specified start to the specified end.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumeration.</typeparam>
		/// <param name="source">The source enumeration.</param>
		/// <param name="start">The start index.</param>
		/// <param name="end">The end index.</param>
		/// <returns>The slice.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static IEnumerable<T> Slice<T>(this IEnumerable<T> source, int start, int end)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));

			return SliceImpl();

			IEnumerable<T> SliceImpl()
			{
				// Optimise item count for ICollection interfaces.
				var count = source is ICollection<T> collection
					? collection.Count
					: (source as ICollection)?.Count ?? source.Count();

				// Get start/end indexes, negative numbers start at the end of the collection.
				if (start < 0)
				{
					start += count;
				}

				if (end < 0)
				{
					end += count;
				}

				var index = 0;
				foreach (var item in source)
				{
					if (index >= end)
					{
						yield break;
					}

					if (index >= start)
					{
						yield return item;
					}

					++index;
				}
			}
		}

		/// <summary>
		/// Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and
		/// Max just aren't enough information. Works with int, double, float.
		/// </summary>
		/// <param name="source">The source enumerable.</param>
		/// <returns>The standard deviation.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static double StdDev(this IEnumerable<int> source) => source.StdDevLogic();

		/// <summary>
		/// Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and
		/// Max just aren't enough information. Works with int, double, float.
		/// </summary>
		/// <param name="source">The source enumerable.</param>
		/// <returns>The standard deviation.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static double StdDev(this IEnumerable<double> source) => source.StdDevLogic();

		/// <summary>
		/// Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and
		/// Max just aren't enough information. Works with int, double, float.
		/// </summary>
		/// <param name="source">The source enumerable.</param>
		/// <returns>The standard deviation.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static float StdDev(this IEnumerable<float> source) => source.StdDevLogic();

		/// <summary>
		/// Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and
		/// Max just aren't enough information. Works with int, double, float.
		/// </summary>
		/// <param name="source">The source enumerable.</param>
		/// <param name="buffer">The buffer amount.</param>
		/// <returns>The standard deviation.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		private static float StdDevLogic(this IEnumerable<float> source, int buffer = 1)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));

			var data = source.ToList();
			var average = data.Average();
			var differences = data.ConvertAll(u => Math.Pow(average - u, 2.0));
			return (float)Math.Sqrt(differences.Sum() / (differences.Count - buffer));
		}

		/// <summary>
		/// Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and
		/// Max just aren't enough information. Works with int, double, float.
		/// </summary>
		/// <param name="source">The source enumerable.</param>
		/// <returns>The standard deviation.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static double StdDevP(this IEnumerable<int> source) => source.StdDevLogic(0);

		/// <summary>
		/// Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and
		/// Max just aren't enough information. Works with int, double, float.
		/// </summary>
		/// <param name="source">The source enumerable.</param>
		/// <returns>The standard deviation.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static double StdDevP(this IEnumerable<double> source) => source.StdDevLogic(0);

		/// <summary>
		/// Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and
		/// Max just aren't enough information. Works with int, double, float.
		/// </summary>
		/// <param name="source">The source enumerable.</param>
		/// <returns>The standard deviation.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static double StdDevP(this IEnumerable<float> source) => source.StdDevLogic(0);

		/// <summary>
		/// Converts the source enumerable to a new collection.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <returns>The output collection.</returns>
		public static Collection<T> ToCollection<T>(this IEnumerable<T> source)
		{
			Contract.Ensures(Contract.Result<Collection<T>>() is not null);

			var collection = new Collection<T>();
			if (source is not null)
			{
				foreach (var item in source)
				{
					collection.Add(item);
				}
			}

			return collection;
		}

		/// <summary>
		/// Returns a comma delimited string representing the values in the enumerable.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <returns>The delimited string.</returns>
		public static string? ToCommaSeparatedValueString<T>(this IEnumerable<T> source) => source?.ToDelimitedString(',');

		/// <summary>
		/// Converts an enumerable to a data table.
		/// </summary>
		/// <typeparam name="T">The type of items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <returns>The data table.</returns>
		public static DataTable ToDataTable<T>(this IEnumerable<T> source)
		{
			Contract.Ensures(Contract.Result<DataTable>() is not null);

			var dtReturn = new DataTable();

			// column names
			PropertyInfo[]? oProps = null;

			if (source is null)
			{
				return dtReturn;
			}

			foreach (var rec in source)
			{
				// Use reflection to get property names, to create table, Only first time, others
				// will follow
				if (oProps is null)
				{
					oProps = rec?.GetType().GetProperties();
					if (oProps is not null)
					{
						foreach (var pi in oProps)
						{
							var colType = pi.PropertyType;

							if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
							{
								colType = colType.GetGenericArguments()[0];
							}

							dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
						}
					}
				}

				var dr = dtReturn.NewRow();

				if (oProps is not null)
				{
					foreach (var pi in oProps)
					{
						dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
					}
				}

				dtReturn.Rows.Add(dr);
			}

			return dtReturn;
		}

		/// <summary>
		/// Returns a delimited string representing the values in the enumerable.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <param name="separator">The character separator.</param>
		/// <returns>The delimited string.</returns>
		public static string? ToDelimitedString<T>(this IEnumerable<T> source, char separator)
		{
			if (source is null)
			{
				return null;
			}

			var result = new StringBuilder();
			_ = source.ForEach(value => result.AppendFormat(CultureInfo.CurrentCulture, "{0}{1}", value, separator));
			return result.ToString(0, result.Length - 1);
		}

		/// <summary>
		/// Converts an enumeration of groupings into a Dictionary of those groupings.
		/// </summary>
		/// <typeparam name="TKey">Key type of the grouping and dictionary.</typeparam>
		/// <typeparam name="TValue">Element type of the grouping and dictionary list.</typeparam>
		/// <param name="groupings">The enumeration of groupings from a GroupBy() clause.</param>
		/// <returns>
		/// A dictionary of groupings such that the key of the dictionary is TKey type and the value
		/// is List of TValue type.
		/// </returns>
		public static Dictionary<TKey, List<TValue>>? ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings) where TKey : notnull
		{
			return groupings?.ToDictionary(
				grouping => grouping is null ? default! : grouping.Key,
				values => values?.ToList() ?? new List<TValue>());
		}

		/// <summary>
		/// Converts an IEnumerable to a HashSet
		/// </summary>
		/// <typeparam name="T">The IEnumerable type</typeparam>
		/// <param name="enumerable">The IEnumerable</param>
		/// <returns>A new HashSet</returns>
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
		{
			Contract.Ensures(Contract.Result<HashSet<T>>() is not null);

			var hs = new HashSet<T>();
			if (enumerable is not null)
			{
				foreach (var item in enumerable)
				{
					_ = hs.Add(item);
				}
			}

			return hs;
		}

		/// <summary>
		/// Converts the enumerable to an observable collection.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <returns>The observable collection.</returns>
		public static ObservableCollection<T> ToObservableCollection<T>(
			 this IEnumerable<T> source) => new ObservableCollection<T>().AddRange(source);

		/// <summary>
		/// Transposes the rows and columns of the specified nested enumerable.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="values">The input values.</param>
		/// <returns>The transposed output.</returns>
		public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> values)
		{
			while (true)
			{
				if (values?.Any() != true)
				{
					return Enumerable.Empty<IEnumerable<T>>();
				}

				if (!values.First().Any())
				{
					values = values.Skip(1);
					continue;
				}

				var val = values.First().First();
				var valSkip = values.First().Skip(1);
				var xss = values.Skip(1);
				var xssList = xss as IList<IEnumerable<T>> ?? xss.ToList();
				return new[]
				{
					new[] {val}.Concat(
						xssList.Select(ht => ht.First()))
				}
				.Concat(
					 new[] { valSkip }
						.Concat(
						  xssList.Select(
								ht => ht.Skip(1)))
				.Transpose());
			}
		}

		/// <summary>
		/// When building a LINQ query, you may need to involve optional filtering criteria. Avoids
		/// if statements when building predicates &amp; lambdas for a query. Useful when you don't
		/// know at compile time whether a filter should apply.
		/// </summary>
		/// <typeparam name="TSource">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <param name="predicate">The predicate for the where clause.</param>
		/// <param name="condition">
		/// If evaluates to <c>true</c> then apply the predicate, else just return the enumerable.
		/// </param>
		/// <returns>The enumerable with the predicate applied if condition evaluated to true.</returns>
		public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, bool condition) => condition ? source.Where(predicate) : source;

		/// <summary>
		/// When building a LINQ query, you may need to involve optional filtering criteria. Avoids
		/// if statements when building predicates &amp; lambdas for a query. Useful when you don't
		/// know at compile time whether a filter should apply.
		/// </summary>
		/// <typeparam name="TSource">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The enumerable.</param>
		/// <param name="predicate">The predicate for the where clause.</param>
		/// <param name="condition">
		/// If evaluates to <c>true</c> then apply the predicate, else just return the enumerable.
		/// </param>
		/// <returns>The enumerable with the predicate applied if condition evaluated to true.</returns>
		public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate, bool condition) => condition ? source.Where(predicate) : source;

		/// <summary>
		/// Returns a lazy evaluated enumerable.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="source">The source enumerable.</param>
		/// <returns>The results.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		private static IEnumerable<T> CacheHelper<T>(IEnumerator<T> source)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));

			var isEmpty = new Lazy<bool>(() => !source.MoveNext());
			var head = new Lazy<T>(() => source.Current);
			var tail = new Lazy<IEnumerable<T>>(() => CacheHelper(source));

			return CacheHelper(isEmpty, head, tail);
		}

		/// <summary>
		/// Returns a lazy evaluated enumerable.
		/// </summary>
		/// <typeparam name="T">The type of the items in the enumerable.</typeparam>
		/// <param name="isEmpty">Whether the enumerable is empty.</param>
		/// <param name="head">The head of the enumerable.</param>
		/// <param name="tail">The tail of the enumerable.</param>
		/// <returns>The results.</returns>
		/// <exception cref="ArgumentNullException">isEmpty or head or tail</exception>
		private static IEnumerable<T> CacheHelper<T>(Lazy<bool> isEmpty, Lazy<T> head, Lazy<IEnumerable<T>> tail)
		{
			_ = isEmpty ?? throw new ArgumentNullException(nameof(isEmpty));
			_ = head ?? throw new ArgumentNullException(nameof(head));
			_ = tail ?? throw new ArgumentNullException(nameof(tail));

			if (isEmpty.Value)
			{
				yield break;
			}

			yield return head.Value;

			if (tail.Value is null)
			{
				yield break;
			}

			foreach (var value in tail.Value)
			{
				yield return value;
			}
		}

		/// <summary>
		/// Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and
		/// Max just aren't enough information. Works with int, double, float.
		/// </summary>
		/// <param name="source">The source enumerable.</param>
		/// <param name="buffer">The buffer amount.</param>
		/// <returns>The standard deviation.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		private static double StdDevLogic(this IEnumerable<double> source, int buffer = 1)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));

			var data = source.ToList();
			var average = data.Average();
			var differences = data.ConvertAll(u => Math.Pow(average - u, 2.0));
			return Math.Sqrt(differences.Sum() / (differences.Count - buffer));
		}

		/// <summary>
		/// Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and
		/// Max just aren't enough information. Works with int, double, float.
		/// </summary>
		/// <param name="source">The source enumerable.</param>
		/// <param name="buffer">The buffer amount.</param>
		/// <returns>The standard deviation.</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		/// ReSharper disable once SuspiciousTypeConversion.Global
		private static double StdDevLogic(this IEnumerable<int> source, int buffer = 1) => source.Cast<double>().StdDevLogic(buffer);
	}
}
