using System.Diagnostics;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlText"/>
[DebuggerDisplay("{Value}")]
public readonly partial struct XmlText : IXmlText
{
	private const string TextName = "#text";

	/// <inheritdoc />
	public string Name => TextName;
	/// <inheritdoc />
	public string? Value { get; }

	/// <inheritdoc cref="XmlBuilder.Text(in string?)"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Text"/> method instead of this constructor</b>
	/// </remarks>
	public XmlText(in string? value = null)
	{
		Value = value;
	}
}