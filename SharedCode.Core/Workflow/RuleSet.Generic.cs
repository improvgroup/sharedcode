
namespace SharedCode.Workflow;

using System.Collections.Generic;

/// <summary>
/// A class that holds a set of rules that can be evaluated.
/// </summary>
/// <typeparam name="T">The type of input to be evaluated.</typeparam>
public class RuleSet<T>
{
	private readonly List<Rule<T>> rules = new();

	/// <summary>
	/// Adds the specified rule to the set.
	/// </summary>
	/// <param name="rule">The rule to be added.</param>
	public void AddRule(Rule<T> rule) => this.rules.Add(rule);

	/// <summary>
	/// Removes the specified rule from the set.
	/// </summary>
	/// <param name="rule">The rule to be removed.</param>
	public void RemoveRule(Rule<T> rule) => this.rules.Remove(rule);

	/// <summary>
	/// Evaluates the specified input against the rules in the set.
	/// </summary>
	/// <param name="input">The input to evaluate.</param>
	/// <returns><c>true</c> if the input satisfies the conditions of all rules in the set, <c>false</c> otherwise.</returns>
	public bool Evaluate(T input) => this.rules.All(rule => rule.IsSatisfied(input));
}
