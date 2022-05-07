// <copyright file="DataReaderExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Text;

/// <summary>
/// The data reader extensions class
/// </summary>
public static class DataReaderExtensions
{
	/// <summary>
	/// Returns an enumerable from this data reader.
	/// </summary>
	/// <param name="dataReader">The data reader.</param>
	/// <returns>An enumerable of data records.</returns>
	public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader dataReader)
	{
		Contract.Ensures(Contract.Result<IEnumerable<IDataRecord>>() != null);

		if (dataReader is null)
			yield break;

		while (dataReader.Read())
		{
			yield return dataReader;
		}
	}

	/// <summary>
	/// Determines whether The specified column value is null.
	/// </summary>
	/// <param name="dataReader">The data reader.</param>
	/// <param name="columnName">Name of the column.</param>
	/// <returns>A value indicating whether or not the specified column is null.</returns>
	public static bool IsDBNull(this IDataReader dataReader, string columnName) => dataReader?.IsDBNull(dataReader.GetOrdinal(columnName)) ?? false;

	/// <summary>
	/// Returns a list of delimited lines from the data reader.
	/// </summary>
	/// <param name="dataReader">The data reader.</param>
	/// <param name="separator">The value separator.</param>
	/// <param name="includeHeaderAsFirstRow">if set to <c>true</c> include header as first row.</param>
	/// <returns>A list of delimited lines.</returns>
	/// <exception cref="ArgumentNullException">dataReader</exception>
	public static IList<string> ToDelimited(this IDataReader dataReader, string separator, bool includeHeaderAsFirstRow)
	{
		_ = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
		Contract.Ensures(Contract.Result<List<string>>() != null);

		var output = new List<string>();
		var sb = new StringBuilder();

		if (includeHeaderAsFirstRow)
		{
			for (var index = 0; index < dataReader.FieldCount; index++)
			{
				if (dataReader.GetName(index) != null)
					_ = sb.Append(dataReader.GetName(index));

				if (index < dataReader.FieldCount - 1)
					_ = sb.Append(separator);
			}

			output.Add(sb.ToString());
		}

		while (dataReader.Read())
		{
			_ = sb.Clear();
			for (var index = 0; index < dataReader.FieldCount - 1; index++)
			{
				if (!dataReader.IsDBNull(index))
				{
					var value = dataReader.GetValue(index).ToString();
					if (dataReader.GetFieldType(index) == typeof(string))
					{
						// If double quotes are used in value, ensure each are replaced but 2.
						if (value?.Contains('"', StringComparison.Ordinal) ?? false)
						{
							value = value.Replace("\"", "\"\"", StringComparison.Ordinal);
						}

						// If separtor are is in value, ensure it is put in double quotes.
						if (value?.Contains(separator, StringComparison.Ordinal) ?? false)
						{
							value = $"\"{value}\"";
						}
					}

					_ = sb.Append(value);
				}

				if (index < dataReader.FieldCount - 1)
					_ = sb.Append(separator);
			}

			if (!dataReader.IsDBNull(dataReader.FieldCount - 1))
			{
				_ = sb.Append(
					dataReader
						.GetValue(dataReader.FieldCount - 1)
						.ToString()
						?.Replace(separator, " ", StringComparison.Ordinal));
			}

			output.Add(sb.ToString());
		}

		dataReader.Close();
		return output;
	}

	/// <summary>
	/// Returns the value of the specified column.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="dataReader">The data reader.</param>
	/// <param name="columnName">Name of the column.</param>
	/// <returns>The value.</returns>
	/// <exception cref="ArgumentNullException">columnName</exception>
	public static T? ValueOrDefault<T>(this IDataReader dataReader, string columnName)
	{
		_ = columnName ?? throw new ArgumentNullException(nameof(columnName));

		var value = dataReader?[columnName];
		return DBNull.Value == value ? default : (T?)value;
	}

	/// <summary>
	/// Returns the value of the specified column.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="dataReader">The data reader.</param>
	/// <param name="columnIndex">Index of the column.</param>
	/// <returns>The value.</returns>
	public static T? ValueOrDefault<T>(this IDataReader dataReader, int columnIndex)
	{
		return (dataReader?.IsDBNull(columnIndex) ?? true) || dataReader?.FieldCount <= columnIndex || columnIndex < 0
			? default
			: (T?)dataReader?[columnIndex];
	}
}
