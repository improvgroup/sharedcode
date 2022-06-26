// <copyright file="DataWidthAttribute.cs" company="improvGroup, LLC">
//     Copyright © 2005-2022 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Attributes
{
	using System;

	/// <summary>
	/// Specifies the input/output width when using a fixed-width format.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public sealed class DataWidthAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataWidthAttribute" /> class.
		/// </summary>
		/// <param name="width">The width of the data when using a fixed-width format.</param>
		public DataWidthAttribute(int width)
		{
			if (width <= 0)
			{
				throw new ArgumentException($"{nameof(width)} must be > 0", nameof(width));
			}

			this.Width = width;
		}

		/// <summary>
		/// Gets the width of data when using a fixed-width format.
		/// </summary>
		/// <value>The width.</value>
		public int Width { get; }
	}
}
