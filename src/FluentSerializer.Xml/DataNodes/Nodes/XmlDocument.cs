using System.Collections.Generic;
using System.Diagnostics;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlDocument"/>
[DebuggerDisplay(DocumentName)]
public readonly partial struct XmlDocument : IXmlDocument
{
	private const string DocumentName = "<?xml ?>";

	/// <inheritdoc />
	public IXmlElement? RootElement { get; }
	/// <inheritdoc />
	public IReadOnlyList<IXmlNode> Children => RootElement?.Children ?? new List<IXmlNode>(0);

	/// <inheritdoc />
	public string Name => DocumentName;

	/// <inheritdoc cref="IXmlDocument"/>
	/// <param name="root">The root element to represent the actual document</param>
	public XmlDocument(in IXmlElement? root)
	{
		RootElement = root;
	}
}