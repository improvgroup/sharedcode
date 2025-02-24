
namespace SharedCode.Specifications.Exceptions;

/// <summary>
/// Class DuplicateSkipException. Implements the <see cref="Exception" />
/// </summary>
/// <seealso cref="Exception" />
public class DuplicateSkipException : Exception
{
	/// <summary>
	/// The message
	/// </summary>
	private const string message = "Duplicate use of the Skip(). Ensure you don't use both Paginate() and Skip() in the same specification!";

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateSkipException" /> class.
	/// </summary>
	public DuplicateSkipException()
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateSkipException" /> class.
	/// </summary>
	/// <param name="innerException">The inner exception.</param>
	public DuplicateSkipException(Exception innerException)
		: base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateSkipException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public DuplicateSkipException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DuplicateSkipException" /> class.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">
	/// The exception that is the cause of the current exception, or a null reference ( <see
	/// langword="Nothing" /> in Visual Basic) if no inner exception is specified.
	/// </param>
	public DuplicateSkipException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
