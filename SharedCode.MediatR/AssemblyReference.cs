namespace SharedCode.MediatR;

using System.Reflection;

/// <summary>
/// The assembly reference class.
/// </summary>
public static class AssemblyReference
{
	/// <summary>
	/// The assembly
	/// </summary>
	public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
