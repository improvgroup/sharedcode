// <copyright file="DynamicDataRecord.cs" company="improvGroup, LLC">
//     Copyright © 2013-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data
{
	using SharedCode.Data.Exceptions;

	using System;
	using System.Data;
	using System.Dynamic;

	/// <summary>
	/// The dynamic data record class.
	/// </summary>
	public class DynamicDataRecord : DynamicObject
	{
		/// <summary>
		/// The record
		/// </summary>
		private readonly IDataRecord record;

		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicDataRecord" /> class.
		/// </summary>
		/// <param name="record">The record.</param>
		public DynamicDataRecord(IDataRecord record) => this.record = record;

		/// <inheritdoc/>
		public override bool TryGetMember(GetMemberBinder binder, out object? result)
		{
			if (binder is null)
			{
				throw new ArgumentNullException(nameof(binder));
			}

			try
			{
				var rawResult = this.record.GetValue(this.record.GetOrdinal(binder.Name));
				result = rawResult == DBNull.Value ? default : rawResult;
				return true;
			}
			catch (IndexOutOfRangeException)
			{
				throw new ColumnIsNotInResultSetException(binder.Name);
			}
		}
	}
}
