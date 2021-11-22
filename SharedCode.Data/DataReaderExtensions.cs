// <copyright file="DataReaderExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;

	/// <summary>
	/// The data reader extensions class.
	/// </summary>
	public static class DataReaderExtensions
	{
		/// <summary>
		/// Enumerates the specified database data reader.
		/// </summary>
		/// <typeparam name="T">The type of the objects being enumerated.</typeparam>
		/// <param name="dbDataReader">The database data reader.</param>
		/// <returns>The enumerable sequence.</returns>
		/// <exception cref="ArgumentNullException">dbDataReader</exception>
		public static IEnumerable<T> Enumerate<T>(this IDataReader dbDataReader) where T : new()
		{
			return dbDataReader is null
				? throw new ArgumentNullException(nameof(dbDataReader))
				: EnumerateImpl();

			IEnumerable<T> EnumerateImpl()
			{
				var converter = new DataRecordConverter<T>(dbDataReader);
				while (dbDataReader.Read())
				{
					yield return converter.ConvertRecordToItem();
				}
			}
		}

		/// <summary>
		/// Enumerates the specified database data reader as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">The type of the objects being enumerated.</typeparam>
		/// <param name="dbDataReader">The database data reader.</param>
		/// <returns>The asynchronous enumerable sequence.</returns>
		/// <exception cref="ArgumentNullException">dbDataReader</exception>
		public static IAsyncEnumerable<T> EnumerateAsync<T>(this DbDataReader dbDataReader) where T : new()
		{
			return dbDataReader is null
				? throw new ArgumentNullException(nameof(dbDataReader))
				: EnumerateAsyncImpl();

			async IAsyncEnumerable<T> EnumerateAsyncImpl()
			{
				var converter = new DataRecordConverter<T>(dbDataReader);
				while (await dbDataReader.ReadAsync().ConfigureAwait(true))
				{
					yield return converter.ConvertRecordToItem();
				}
			}
		}

		/// <summary>
		/// Maps the specified database data record to the specified object type <typeparamref
		/// name="T" />.
		/// </summary>
		/// <typeparam name="T">The type of the object being mapped.</typeparam>
		/// <param name="dbDataRecord">The database data record.</param>
		/// <returns>The resulting object.</returns>
		/// <exception cref="ArgumentNullException">dbDataRecord</exception>
		public static T Map<T>(this IDataRecord dbDataRecord) where T : new()
		{
			if (dbDataRecord is null)
			{
				throw new ArgumentNullException(nameof(dbDataRecord));
			}

			var converter = new DataRecordConverter<T>(dbDataRecord);
			return converter.ConvertRecordToItem();
		}
	}
}
