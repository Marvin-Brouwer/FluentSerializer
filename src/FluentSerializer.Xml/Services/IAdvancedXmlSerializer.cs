using FluentSerializer.Xml.DataNodes;
using System;

namespace FluentSerializer.Xml.Services;

/// <summary>
/// The FluentSerializer for XML with overloads for more advance use-cases
/// </summary>
public interface IAdvancedXmlSerializer : IXmlSerializer
{
	/// <summary>
	/// Serialize <paramref name="model"/> to a node representation
	/// </summary>
	IXmlElement? SerializeToElement(in object? model, in Type modelType);

	/// <summary>
	/// Deserialize <paramref name="element"/> from a node representation to an object
	/// </summary>
	object? Deserialize(in IXmlElement element, in Type modelType);
}