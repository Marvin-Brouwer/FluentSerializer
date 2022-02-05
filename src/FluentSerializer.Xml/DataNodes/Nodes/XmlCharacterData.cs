using FluentSerializer.Core.DataNodes;
using FluentSerializer.Xml.Configuration;
using System;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlCharacterData"/>
[DebuggerDisplay(CharacterDataName)]
public readonly struct XmlCharacterData : IXmlCharacterData
{
	private static readonly int TypeHashCode = typeof(XmlCharacterData).GetHashCode();

	private const string CharacterDataName = "<![CDATA[ ]]>";
	/// <inheritdoc />
	public string Name => CharacterDataName;
	/// <inheritdoc />
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

	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		return stringBuilder
			.Append(XmlCharacterConstants.CharacterDataStart)
			.Append(Value)
			.Append(XmlCharacterConstants.CharacterDataEnd);
	}

	#region IEquatable

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

	#endregion
}