using Ardalis.GuardClauses;

using FluentSerializer.Core.Extensions;

using System.Diagnostics;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlAttribute"/>
[DebuggerDisplay("{Name,nq}={Value}")]
public readonly partial struct XmlAttribute : IXmlAttribute
{
	/// <inheritdoc />
	public string Name { get; }
	/// <inheritdoc />
	public string Value { get; }

	/// <inheritdoc cref="XmlBuilder.Attribute(in string, in string?)"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Attribute"/> method instead of this constructor</b>
	/// </remarks>
	public XmlAttribute(in string name, in string value)
	{
		Guard.Against.InvalidName(in name);

		Name = name;
		Value = value;
	}
}