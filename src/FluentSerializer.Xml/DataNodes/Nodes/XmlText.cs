using FluentSerializer.Core.DataNodes;
using FluentSerializer.Xml.Configuration;
using System;
using System.Diagnostics;
using FluentSerializer.Core.Text;
using FluentSerializer.Core.Text.Extensions;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlText"/>
[DebuggerDisplay("{Value}")]
public readonly struct XmlText : IXmlText
{
	private static readonly int TypeHashCode = typeof(XmlText).GetHashCode();

	private const string TextName = "#text";

	public string Name => TextName;
	public string? Value { get; }

	/// <inheritdoc cref="XmlBuilder.Text(string?)"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Text"/> method instead of this constructor</b>
	/// </remarks>
	public XmlText(string? value = null)
	{
		Value = value;
	}

	/// <inheritdoc cref="IXmlText"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public XmlText(in ReadOnlySpan<char> text, ref int offset)
	{
		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			valueEndOffset = offset;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter)) break;

			offset++;
		}

		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
	}

	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		// This should never happen because null tags are self-closing but just to be sure this check is here
		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		return stringBuilder.Append(Value);
	}

	#region IEquatable

	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

	#endregion
}