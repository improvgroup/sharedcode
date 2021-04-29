// <copyright file="ColumnIsNotInResultSetException.cs" company="improvGroup, LLC">
//     Copyright © 2013-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data.Exceptions
{
	using System;

	/// <summary>
	/// The column is not in result set exception class.
	/// </summary>
	public class ColumnIsNotInResultSetException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnIsNotInResultSetException" /> class.
		/// </summary>
		public ColumnIsNotInResultSetException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnIsNotInResultSetException" /> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public ColumnIsNotInResultSetException(string name)
			: base(name)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnIsNotInResultSetException" /> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The exception.</param>
		public ColumnIsNotInResultSetException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
