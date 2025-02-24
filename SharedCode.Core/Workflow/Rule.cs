
namespace SharedCode.Workflow;

/// <summary>
/// The rule class.
/// </summary>
public class Rule : Rule<object?>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Rule"/> class.
	/// </summary>
	/// <param name="condition">The function used to evaluate the rule condition; returns true if passed, false otherwise.</param>
	public Rule(Func<object?, bool> condition) : base(condition)
	{
	}
}
