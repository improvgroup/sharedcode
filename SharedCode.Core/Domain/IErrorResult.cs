namespace SharedCode.Domain;
/// <summary>
/// The error result interface.
/// </summary>
internal interface IErrorResult
{
	/// <summary>
	/// Gets the errors.
	/// </summary>
	/// <value>The errors.</value>
	IReadOnlyCollection<Error> Errors { get; }

	/// <summary>
	/// Gets the message.
	/// </summary>
	/// <value>The message.</value>
	string Message { get; }
}
