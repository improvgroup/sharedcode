// <copyright file="XDocumentExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Xml
{
	using System.Xml.Linq;
	using System.Xml.Serialization;

	/// <summary>
	/// The <see cref="XDocument" /> extensions class.
	/// </summary>
	public static class XDocumentExtensions
	{
		/// <summary>
		/// Deserializes the specified XML document.
		/// </summary>
		/// <typeparam name="T">The type represented in the XML document.</typeparam>
		/// <param name="xmlDocument">The XML document.</param>
		/// <returns>The deserialized object.</returns>
		/// <exception cref="ArgumentNullException">xmlDocument</exception>
		public static T? Deserialize<T>(this XDocument xmlDocument)
		{
			_ = xmlDocument ?? throw new ArgumentNullException(nameof(xmlDocument));

			var xmlSerializer = new XmlSerializer(typeof(T));
			using var reader = xmlDocument.CreateReader();
			return (T?)xmlSerializer.Deserialize(reader);
		}
	}
}
