namespace SharedCode.Domain;

/// <summary>
///   <para>The result class.</para>
/// </summary>
/// <example>
/// Here are some usage examples...
/// <code title="Basic Usage">var result = GetData();
/// if(result.Success)
/// {
///  // Do something
/// }
/// else
/// {
///  // Handle error
/// }</code><code title="Including Error Context">var result = GetData();
///
/// return result switch
/// {
///     SuccessResult&lt;Hamburger&gt; successResult =&gt; HandleSuccess(successResult),
///     NotFoundResult&lt;Hamburger&gt; notFoundResult =&gt; HandleNotFoundResult(notFoundResult),
///     ErrorResult&lt;Hamburger&gt; errorResult =&gt; HandleErrorResult(errorResult),
///         _ =&gt; result.MissingPatternMatch();
/// };</code><code title="Failure with Specific Result Type">var result = GetData();
///
/// if(result.Failure)
/// {
///     if(result is NotFoundResult notFoundResult)
///     {
///        // Handle not found result
///     }
///     // Handle "unknown" error
/// }</code></example>
public abstract class Result
{
	/// <summary>
	/// Gets a value indicating whether this <see cref="Result" /> is failure.
	/// </summary>
	/// <value><c>true</c> if failure; otherwise, <c>false</c>.</value>
	public bool Failure => !this.Success;

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="Result" /> is success.
	/// </summary>
	/// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
	public bool Success { get; protected set; }
}
