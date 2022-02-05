using System.Collections.Generic;

namespace FluentSerializer.Xml.DataNodes;

/// <summary>
/// A representation of an XML element <br/>
/// <see href="https://www.w3.org/TR/xml/#elemdecls" /> <br/><br/>
/// <code><![CDATA[<Name></Name>]]></code>
/// </summary>
public interface IXmlElement : IXmlContainer<IXmlElement>
{
	/// <summary>
	/// Find an attribute by name if present
	/// </summary>
	IXmlAttribute? GetChildAttribute(in string name);

	/// <summary>
	/// List all children with a given name
	/// </summary>
	IEnumerable<IXmlElement> GetChildElements(string? name = null);
	/// <summary>
	/// Find an element by name if present
	/// </summary>
	IXmlElement? GetChildElement(in string name);

	/// <summary>
	/// Get the combined text value of all text nodes
	/// </summary>
	/// <returns></returns>
	string? GetTextValue();
}