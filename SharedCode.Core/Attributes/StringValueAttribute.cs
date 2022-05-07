// <copyright file="StringValueAttribute.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Attributes;

using System;

/// <summary>
/// The string value attribute class
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public sealed class StringValueAttribute : Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="StringValueAttribute" /> class.
	/// </summary>
	/// <param name="value">The string value.</param>
	public StringValueAttribute(string value) => this.Value = value ?? throw new ArgumentNullException(nameof(value));

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <value>The value.</value>
	public string Value { get; }
}
