
using SharedCode.Problems;

namespace SharedCode;
/// <summary>
/// Represents a result that contains a problem.
/// </summary>
public interface  IResultProblem
{
	/// <summary>
	/// Gets or sets the problem details.
	/// </summary>
	Problem? Problem { get; set; }

	/// <summary>
	/// Gets a value indicating whether the result has a problem.
	/// </summary>
	bool HasProblem { get; }

	/// <summary>
	/// Throws an exception if the result indicates a failure.
	/// </summary>
	/// <returns><see langword="true"/> if the operation succeeded; otherwise, <see langword="false"/>.</returns>
	bool ThrowIfFailure();

	/// <summary>
	/// Tries to get the problem details from the result.
	/// </summary>
	/// <param name="problem">
	/// The problem details if available; otherwise, <see langword="null"/>.
	/// </param>
	/// <returns>
	/// <c>true</c> if the problem details were successfully retrieved; otherwise, <c>false</c>.
	/// </returns>
	bool TryGetProblem([MaybeNullWhen(false)] out Problem problem);
}
