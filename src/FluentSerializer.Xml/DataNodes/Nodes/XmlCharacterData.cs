using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Configuration;
using System;
using System.Diagnostics;
using FluentSerializer.Core.Text;
using FluentSerializer.Core.Text.Extensions;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlCharacterData"/>
[DebuggerDisplay(CharacterDataName)]
public readonly struct XmlCharacterData : IXmlCharacterData
{
	private static readonly int TypeHashCode = typeof(XmlCharacterData).GetHashCode();

	private const string CharacterDataName = "<![CDATA[ ]]>";
	public string Name => CharacterDataName;
	public string? Value { get; }

	/// <inheritdoc cref="XmlBuilder.CData(string)"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.CData"/> method instead of this constructor</b>
	/// </remarks>
	public XmlCharacterData(string? value = null)
	{
		Value = value;
	}

	/// <inheritdoc cref="IXmlCharacterData"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public XmlCharacterData(in ReadOnlySpan<char> text, ref int offset)
	{
		offset.AdjustForToken(XmlCharacterConstants.CharacterDataStart);

		var valueStartOffset = offset;
		var valueEndOffset = offset;
            
		while (text.WithinCapacity(in offset))
		{
			valueEndOffset = offset;
			if (text.HasCharactersAtOffset(in offset, XmlCharacterConstants.CharacterDataEnd))
			{
				offset.AdjustForToken(XmlCharacterConstants.CharacterDataEnd);
				break;
			}
                
			offset++;
		}

		Value = text[valueStartOffset..valueEndOffset].ToString();
	}

	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		return stringBuilder
			.Append(XmlCharacterConstants.CharacterDataStart)
			.Append(Value)
			.Append(XmlCharacterConstants.CharacterDataEnd);
	}

	#region IEquatable

	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

	#endregion
}