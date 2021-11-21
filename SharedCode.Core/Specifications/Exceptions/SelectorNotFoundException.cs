// <copyright file="SelectorNotFoundException.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Exceptions;

/// <summary>
/// Class SelectorNotFoundException. Implements the <see cref="Exception" />
/// </summary>
/// <seealso cref="Exception" />
public class SelectorNotFoundException : Exception
{
	/// <summary>
	/// The message
	/// </summary>
	private const string message = "The specification must have Selector defined.";

	/// <summary>
	/// Initializes a new instance of the <see cref="SelectorNotFoundException" /> class.
	/// </summary>
	public SelectorNotFoundException()
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SelectorNotFoundException" /> class.
	/// </summary>
	/// <param name="innerException">The inner exception.</param>
	public SelectorNotFoundException(Exception innerException)
		: base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SelectorNotFoundException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public SelectorNotFoundException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SelectorNotFoundException" /> class.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">
	/// The exception that is the cause of the current exception, or a null reference ( <see
	/// langword="Nothing" /> in Visual Basic) if no inner exception is specified.
	/// </param>
	public SelectorNotFoundException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
