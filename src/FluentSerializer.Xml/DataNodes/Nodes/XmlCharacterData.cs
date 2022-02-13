using System.Diagnostics;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlCharacterData"/>
[DebuggerDisplay(CharacterDataName)]
public readonly partial struct XmlCharacterData : IXmlCharacterData
{
	private const string CharacterDataName = "<![CDATA[ ]]>";

	/// <inheritdoc />
	public string Name => CharacterDataName;
	/// <inheritdoc />
	public string? Value { get; }

	/// <inheritdoc cref="XmlBuilder.CData(in string)"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.CData"/> method instead of this constructor</b>
	/// </remarks>
	public XmlCharacterData(in string? value = null)
	{
		Value = value;
	}
}