namespace SharedCode.Domain;

/// <summary>
/// The validation error result class. Implements the <see cref="ErrorResult" />.
/// </summary>
/// <seealso cref="ErrorResult" />
public class ValidationErrorResult : ErrorResult
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ValidationErrorResult" /> class.
	/// </summary>
	/// <param name="message">The message.</param>
	public ValidationErrorResult(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ValidationErrorResult" /> class.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <param name="errors">The errors.</param>
	public ValidationErrorResult(string message, IReadOnlyCollection<ValidationError> errors) : base(message, errors)
	{
	}
}
