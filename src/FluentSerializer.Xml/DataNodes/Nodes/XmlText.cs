using FluentSerializer.Core.DataNodes;
using FluentSerializer.Xml.Configuration;
using System;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlText"/>
[DebuggerDisplay("{Value}")]
public readonly struct XmlText : IXmlText
{
	private static readonly int TypeHashCode = typeof(XmlText).GetHashCode();

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

	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		// This should never happen because null tags are self-closing but just to be sure this check is here
		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		return stringBuilder.Append(Value);
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