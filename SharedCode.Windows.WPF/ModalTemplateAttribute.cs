namespace SharedCode.Windows.WPF;

using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The modal template attribute class.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public sealed class ModalTemplateAttribute : Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ModalTemplateAttribute" /> class.
	/// </summary>
	/// <param name="type">The modal template type.</param>
	[SuppressMessage("Design", "CA1019:Define accessors for attribute arguments", Justification = "False positive.")]
	public ModalTemplateAttribute(Type type) => this.Template = type;

	/// <summary>
	/// Gets the template type.
	/// </summary>
	/// <value>The template type.</value>
	public Type Template { get; }
}
