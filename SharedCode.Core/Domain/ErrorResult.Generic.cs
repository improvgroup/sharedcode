namespace SharedCode.Domain;
/// <summary>
/// The error result class. Implements the <see cref="Result{T}" />. Implements the <see
/// cref="IErrorResult" />.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
/// <seealso cref="Result{T}" />
/// <seealso cref="IErrorResult" />
public class ErrorResult<T> : Result<T>, IErrorResult
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ErrorResult{T}" /> class.
	/// </summary>
	/// <param name="message">The error message.</param>
	public ErrorResult(string message) : this(message, Array.Empty<Error>())
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ErrorResult{T}" /> class.
	/// </summary>
	/// <param name="message">The error message.</param>
	/// <param name="errors">The list of errors.</param>
	public ErrorResult(string message, IReadOnlyCollection<Error> errors) : base(default)
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
	/// Gets or sets the message.
	/// </summary>
	/// <value>The message.</value>
	public string Message { get; set; }
}
