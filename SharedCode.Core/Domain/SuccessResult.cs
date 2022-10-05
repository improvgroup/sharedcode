namespace SharedCode.Domain;

/// <summary>
/// The successful result class. Implements the <see cref="Result" />.
/// </summary>
/// <seealso cref="Result" />
public class SuccessResult : Result
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SuccessResult" /> class.
	/// </summary>
	public SuccessResult() => this.Success = true;
}
