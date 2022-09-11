// <copyright file="DataTableExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data
{
	using System;
	using System.Data;
	using System.Diagnostics.Contracts;
	using System.Text;
	using System.Xml.Linq;

	/// <summary>
	/// The data table extensions class.
	/// </summary>
	public static class DataTableExtensions
	{
		/// <summary>
		/// Converts a data table to a delimited string.
		/// </summary>
		/// <param name="this">The data table.</param>
		/// <param name="delimiter">The column delimiter.</param>
		/// <param name="includeHeader">if set to <c>true</c> [include header].</param>
		/// <exception cref="ArgumentNullException">table</exception>
		[SuppressMessage("Refactoring", "GCop659:Use 'var' instead of explicit type.", Justification = "The compiler does not infer DataColumn properly here so we have to use the type instead of var.")]
		public static string ToDelimitedString(this DataTable @this, string delimiter, bool includeHeader)
		{
			ArgumentNullException.ThrowIfNull(@this);
			Contract.Ensures(Contract.Result<string>() is not null);

			var result = new StringBuilder();

			_ = result.Append(string.Empty);

			if (includeHeader)
			{
				foreach (DataColumn column in @this.Columns)
				{
					_ = result.Append(column.ColumnName).Append(delimiter);
				}

				_ = result.Remove(--result.Length, 0).Append(Environment.NewLine);
			}

			foreach (DataRow row in @this.Rows)
			{
				foreach (var item in row.ItemArray)
				{
					if (item is DBNull)
					{
						_ = result.Append(delimiter);
					}
					else
					{
						// Double up all embedded double quotes To keep things simple, always
						// delimit with double-quotes so we don't have to determine in which cases
						// they're necessary and which cases they're not.
						_ = result.Append('"').Append(item?.ToString()?.Replace("\"", "\"\"", StringComparison.Ordinal)).Append('"').Append(delimiter);
					}
				}

				_ = result
					.Remove(result.Length - 1, 1)
					.Append(Environment.NewLine);
			}

			return result.ToString();
		}

		/// <summary>
		/// Converts the data table to XML.
		/// </summary>
		/// <param name="this">The data table.</param>
		/// <param name="rootName">Name of the XML root node.</param>
		/// <returns>An XML document.</returns>
		/// <exception cref="ArgumentNullException">dataTable or rootName</exception>
		[SuppressMessage("Refactoring", "GCop659:Use 'var' instead of explicit type.", Justification = "The compiler does not infer DataColumn properly here so we have to use the type instead of var.")]
		public static XDocument ToXml(this DataTable @this, string rootName)
		{
			ArgumentNullException.ThrowIfNull(@this);
			ArgumentNullException.ThrowIfNull(rootName);
			Contract.Ensures(Contract.Result<XDocument>() is not null);

			var xdoc = new XDocument
			{
				Declaration = new XDeclaration("1.0", "utf-8", "")
			};

			xdoc.Add(new XElement(rootName));

			foreach (DataRow row in @this.Rows)
			{
				var element = new XElement(@this.TableName);
				foreach (DataColumn col in @this.Columns)
				{
					element.Add(new XElement(col.ColumnName, row[col].ToString()?.Trim(' ')));
				}

				xdoc.Root?.Add(element);
			}

			return xdoc;
		}
	}
}
