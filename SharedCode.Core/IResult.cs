namespace SharedCode
{
	/// <summary>
	/// Represents a result of an operation.
	/// </summary>
	public interface IResult : IResultInvalid, IResultProblem
	{
		/// <summary>
		/// Gets a value indicating whether the result is successful.
		/// </summary>
		/// <value>
		/// <c>true</c> if the result is successful; otherwise, <c>false</c>.
		/// </value>
		bool IsSuccess { get; }

		/// <summary>
		/// Gets a value indicating whether the result is a failure.
		/// </summary>
		/// <value>
		/// <c>true</c> if the result is a failure; otherwise, <c>false</c>.
		/// </value>
		bool IsFailure { get; }
	}

	/// <summary>
	/// Represents a result of an operation that contains a value of a specific type.
	/// This interface extends <see cref="IResult"/> and provides access to the value of
	/// the result.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the value contained in the result.
	/// </typeparam>
	public interface IResult<out T> : IResult
	{
		/// <summary>
		/// Gets the value of the result.
		/// </summary>
		/// <value>
		/// The value of the result, or <c>null</c> if the result is empty.
		/// </value>
		T? Value { get; }

		/// <summary>
		/// Gets a value indicating whether the result is empty.
		/// </summary>
		/// <value>
		/// <c>true</c> if the result is empty; otherwise, <c>false</c>.
		/// </value>
		bool IsEmpty { get; }
	}
}
