
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
	/// <param name="this">The data reader.</param>
	/// <returns>An enumerable of data records.</returns>
	public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader @this)
	{
		Contract.Ensures(Contract.Result<IEnumerable<IDataRecord>>() is not null);

		if (@this is null)
		{
			yield break;
		}

		while (@this.Read())
		{
			yield return @this;
		}
	}

	/// <summary>
	/// Determines whether The specified column value is null.
	/// </summary>
	/// <param name="this">The data reader.</param>
	/// <param name="columnName">Name of the column.</param>
	/// <returns>A value indicating whether or not the specified column is null.</returns>
	public static bool IsDBNull(this IDataReader @this, string columnName) => @this?.IsDBNull(@this.GetOrdinal(columnName)) ?? false;

	/// <summary>
	/// Returns a list of delimited lines from the data reader.
	/// </summary>
	/// <param name="this">The data reader.</param>
	/// <param name="separator">The value separator.</param>
	/// <param name="includeHeaderAsFirstRow">if set to <c>true</c> include header as first row.</param>
	/// <returns>A list of delimited lines.</returns>
	/// <exception cref="ArgumentNullException">dataReader</exception>
	public static IList<string> ToDelimited(this IDataReader @this, string separator, bool includeHeaderAsFirstRow)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		Contract.Ensures(Contract.Result<List<string>>() is not null);

		var output = new List<string>();
		var sb = new StringBuilder();

		if (includeHeaderAsFirstRow)
		{
			for (var index = 0; index < @this.FieldCount; index++)
			{
				if (@this.GetName(index) is not null)
				{
					_ = sb.Append(@this.GetName(index));
				}

				if (index < @this.FieldCount - 1)
				{
					_ = sb.Append(separator);
				}
			}

			output.Add(sb.ToString());
		}

		while (@this.Read())
		{
			_ = sb.Clear();
			for (var index = 0; index < @this.FieldCount - 1; index++)
			{
				if (!@this.IsDBNull(index))
				{
					var value = @this.GetValue(index).ToString();
					if (@this.GetFieldType(index) == typeof(string))
					{
						// If double quotes are used in value, ensure each are replaced but 2.
#if NET6_0_OR_GREATER
						if (value?.Contains('"', StringComparison.Ordinal) == true)
						{
							value = value.Replace("\"", "\"\"", StringComparison.Ordinal);
						}

						// If separtor are is in value, ensure it is put in double quotes.
						if (value?.Contains(separator, StringComparison.Ordinal) == true)
						{
							value = $"\"{value}\"";
						}
#else
						if (value?.Contains('"') == true)
						{
							value = value.Replace("\"", "\"\"");
						}

						// If separtor are is in value, ensure it is put in double quotes.
						if (value?.Contains(separator) == true)
						{
							value = $"\"{value}\"";
						}
#endif
					}

					_ = sb.Append(value);
				}

				if (index < @this.FieldCount - 1)
				{
					_ = sb.Append(separator);
				}
			}

			if (!@this.IsDBNull(@this.FieldCount - 1))
			{
#if NET6_0_OR_GREATER
				_ = sb.Append(
					@this
						.GetValue(@this.FieldCount - 1)
						.ToString()
						?.Replace(separator, " ", StringComparison.Ordinal));
#else
				_ = sb.Append(
					@this
						.GetValue(@this.FieldCount - 1)
						.ToString()
						?.Replace(separator, " "));
#endif
			}

			output.Add(sb.ToString());
		}

		@this.Close();
		return output;
	}

	/// <summary>
	/// Returns the value of the specified column.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="this">The data reader.</param>
	/// <param name="columnName">Name of the column.</param>
	/// <returns>The value.</returns>
	/// <exception cref="ArgumentNullException">columnName</exception>
	public static T? ValueOrDefault<T>(this IDataReader @this, string columnName)
	{
		_ = columnName ?? throw new ArgumentNullException(nameof(columnName));

		var value = @this?[columnName];
		return DBNull.Value == value ? default : (T?)value;
	}

	/// <summary>
	/// Returns the value of the specified column.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="this">The data reader.</param>
	/// <param name="columnIndex">Index of the column.</param>
	/// <returns>The value.</returns>
	public static T? ValueOrDefault<T>(this IDataReader @this, int columnIndex)
	{
		return (@this?.IsDBNull(columnIndex) ?? true) || @this?.FieldCount <= columnIndex || columnIndex < 0
			? default
			: (T?)@this?[columnIndex];
	}
}
