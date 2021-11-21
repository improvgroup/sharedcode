// <copyright file="DuplicateTakeException.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Exceptions;

/// <summary>
/// Class DuplicateTakeException. Implements the <see cref="System.Exception" />
/// </summary>
/// <seealso cref="System.Exception" />
public class DuplicateTakeException : Exception
{
	/// <summary>
	/// The message
	/// </summary>
	private const string message = "Duplicate use of Take(). Ensure you don't use both Paginate() and Take() in the same specification!";

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateTakeException" /> class.
	/// </summary>
	public DuplicateTakeException()
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateTakeException" /> class.
	/// </summary>
	/// <param name="innerException">The inner exception.</param>
	public DuplicateTakeException(Exception innerException)
		: base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateTakeException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public DuplicateTakeException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateTakeException" /> class.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">
	/// The exception that is the cause of the current exception, or a null reference ( <see
	/// langword="Nothing" /> in Visual Basic) if no inner exception is specified.
	/// </param>
	public DuplicateTakeException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
