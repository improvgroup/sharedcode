namespace SharedCode.Specifications;
/// <summary>
/// A marker interface for specifications that are meant to return a single entity. Used to
/// constrain methods that accept a Specification and return a single result rather than a
/// collection of results
/// </summary>
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is a marker interface.")]
public interface ISingleResultSpecification
{
}
