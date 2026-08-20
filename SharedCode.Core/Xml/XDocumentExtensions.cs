
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SharedCode.Xml;
/// <summary>
/// The <see cref="XDocument" /> extensions class.
/// </summary>
public static class XDocumentExtensions
{
    /// <summary>
    /// Cached XML serializers by runtime type.
    /// </summary>
    private static readonly Dictionary<RuntimeTypeHandle, XmlSerializer> XmlSerializers = [];

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

		var xmlSerializer = GetXmlSerializer(typeof(T));
		using var reader = xmlDocument.CreateReader();
		return (T?)xmlSerializer.Deserialize(reader);
	}

    /// <summary>
    /// Gets the XML serializer for the specified <paramref name="type" />.
    /// </summary>
    /// <param name="type">The type handled by the serializer.</param>
    /// <returns>The <see cref="XmlSerializer" /> for the <paramref name="type" />.</returns>
    /// <exception cref="ArgumentNullException">type</exception>
    private static XmlSerializer GetXmlSerializer(Type type)
    {
        _ = type ?? throw new ArgumentNullException(nameof(type));

        lock (XmlSerializers)
        {
            if (XmlSerializers.TryGetValue(type.TypeHandle, out var serializer))
            {
                return serializer;
            }

            serializer = new XmlSerializer(type);
            XmlSerializers.Add(type.TypeHandle, serializer);

            return serializer;
        }
    }
}
