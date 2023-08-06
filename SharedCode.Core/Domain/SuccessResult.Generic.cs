namespace SharedCode.Domain;

/// <summary>
/// The successful result class. Implements the <see cref="Result{T}" />.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
/// <seealso cref="Result{T}" />
public class SuccessResult<T> : Result<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SuccessResult{T}" /> class.
	/// </summary>
	/// <param name="data">The result data.</param>
	public SuccessResult(T data) : base(data) => this.Success = true;
}
