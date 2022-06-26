// <copyright file="DataRowExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

using System.Data;
using System.Diagnostics.Contracts;

/// <summary>
/// The data row extensions class
/// </summary>
public static class DataRowExtensions
{
	/// <summary>
	/// Creates a cloned and detached copy of a DataRow instance
	/// </summary>
	/// <typeparam name="T">The type of the DataRow if strongly typed</typeparam>
	/// <param name="dataRow">The data row.</param>
	/// <param name="parentTable">The parent table.</param>
	/// <returns>An instance of the new DataRow</returns>
	/// <exception cref="ArgumentNullException">dataRow</exception>
	/// <exception cref="ArgumentNullException">parentTable</exception>
	public static T Clone<T>(this DataRow dataRow, DataTable parentTable)
		where T : DataRow
	{
		_ = dataRow ?? throw new ArgumentNullException(nameof(dataRow));
		_ = parentTable ?? throw new ArgumentNullException(nameof(parentTable));
		Contract.Ensures(Contract.Result<T>() is not null);

		var result = (T)parentTable.NewRow();
		result.ItemArray = dataRow.ItemArray;
		return result;
	}
}
