namespace SharedCode.Windows.WPF
{
	using System;

	/// <summary>
	/// The modal template attribute class.
	/// </summary>
	public class ModalTemplateAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ModalTemplateAttribute" /> class.
		/// </summary>
		/// <param name="type">The type.</param>
		public ModalTemplateAttribute(Type type) => this.Template = type;

		/// <summary>
		/// Gets the template.
		/// </summary>
		/// <value>The template.</value>
		public Type Template { get; }
	}
}
