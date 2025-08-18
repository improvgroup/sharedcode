namespace SharedCode.Domain;
/// <summary>
/// The error result class. Implements the <see cref="Result" />. Implements the <see
/// cref="IErrorResult" />.
/// </summary>
/// <seealso cref="Result" />
/// <seealso cref="IErrorResult" />
public class ErrorResult : Result, IErrorResult
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ErrorResult" /> class.
	/// </summary>
	/// <param name="message">The error message.</param>
	public ErrorResult(string message) : this(message, Array.Empty<Error>())
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ErrorResult" /> class.
	/// </summary>
	/// <param name="message">The error message.</param>
	/// <param name="errors">The list of errors.</param>
	public ErrorResult(string message, IReadOnlyCollection<Error> errors)
	{
		this.Message = message;
		this.Success = false;
		this.Errors = errors ?? Array.Empty<Error>();
	}

	/// <summary>
	/// Gets the errors.
	/// </summary>
	/// <value>The errors.</value>
	public IReadOnlyCollection<Error> Errors { get; }

	/// <summary>
	/// Gets the message.
	/// </summary>
	/// <value>The message.</value>
	public string Message { get; }
}
