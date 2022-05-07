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
		/// <param name="table">The data table.</param>
		/// <param name="delimiter">The column delimiter.</param>
		/// <param name="includeHeader">if set to <c>true</c> [include header].</param>
		/// <exception cref="ArgumentNullException">table</exception>
		public static string ToDelimitedString(this DataTable table, string delimiter, bool includeHeader)
		{
			_ = table ?? throw new ArgumentNullException(nameof(table));
			Contract.Ensures(Contract.Result<string>() != null);

			var result = new StringBuilder();

			_ = result.Append(string.Empty);

			if (includeHeader)
			{
				foreach (DataColumn column in table.Columns)
				{
					_ = result.Append(column.ColumnName).Append(delimiter);
				}

				_ = result.Remove(--result.Length, 0).Append(Environment.NewLine);
			}

			foreach (DataRow row in table.Rows)
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
		/// <param name="dataTable">The data table.</param>
		/// <param name="rootName">Name of the XML root node.</param>
		/// <returns>An XML document.</returns>
		/// <exception cref="ArgumentNullException">dataTable or rootName</exception>
		public static XDocument ToXml(this DataTable dataTable, string rootName)
		{
			_ = dataTable ?? throw new ArgumentNullException(nameof(dataTable));
			_ = rootName ?? throw new ArgumentNullException(nameof(rootName));
			Contract.Ensures(Contract.Result<XDocument>() is not null);

			var xdoc = new XDocument
			{
				Declaration = new XDeclaration("1.0", "utf-8", "")
			};

			xdoc.Add(new XElement(rootName));

			foreach (DataRow row in dataTable.Rows)
			{
				var element = new XElement(dataTable.TableName);
				foreach (DataColumn col in dataTable.Columns)
				{
					element.Add(new XElement(col.ColumnName, row[col].ToString()?.Trim(' ')));
				}

				xdoc.Root?.Add(element);
			}

			return xdoc;
		}
	}
}
