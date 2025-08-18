namespace SharedCode.Workflow;
/// <summary>
/// The rule class.
/// </summary>
/// <typeparam name="T">The type of input on which the rule operates.</typeparam>
public class Rule<T>
{
	private readonly Func<T, bool> condition;

	/// <summary>
	/// Initializes a new instance of the <see cref="Rule{T}" /> class.
	/// </summary>
	/// <param name="condition">The function used to evaluate the rule condition; returns true if passed, false otherwise.</param>
	public Rule(Func<T, bool> condition) => this.condition = condition;

	/// <summary>
	/// Determines whether the specified input satisfies the condition.
	/// </summary>
	/// <param name="input">The input to be evaluated.</param>
	/// <returns><c>true</c> if the specified input is satisfies the condition; otherwise, <c>false</c>.</returns>
	public bool IsSatisfied(T input) => this.condition(input);
}
