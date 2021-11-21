// <copyright file="DuplicateOrderChainException.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Exceptions;

/// <summary>
/// Class DuplicateOrderChainException. Implements the <see cref="Exception" />
/// </summary>
/// <seealso cref="Exception" />
public class DuplicateOrderChainException : Exception
{
	/// <summary>
	/// The message
	/// </summary>
	private const string message = "The specification contains more than one Order chain!";

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateOrderChainException" /> class.
	/// </summary>
	public DuplicateOrderChainException()
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateOrderChainException" /> class.
	/// </summary>
	/// <param name="innerException">The inner exception.</param>
	public DuplicateOrderChainException(Exception innerException)
		: base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateOrderChainException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public DuplicateOrderChainException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateOrderChainException" /> class.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">
	/// The exception that is the cause of the current exception, or a null reference ( <see
	/// langword="Nothing" /> in Visual Basic) if no inner exception is specified.
	/// </param>
	public DuplicateOrderChainException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
