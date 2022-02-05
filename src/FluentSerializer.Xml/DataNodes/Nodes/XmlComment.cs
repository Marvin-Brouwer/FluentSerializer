using FluentSerializer.Core.DataNodes;
using FluentSerializer.Xml.Configuration;
using System;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlComment"/>
[DebuggerDisplay("<!-- {Value, nq} -->")]
public readonly struct XmlComment : IXmlComment
{
	private static readonly int TypeHashCode = typeof(XmlComment).GetHashCode();

	private const string CommentName = "<!-- comment -->";
	/// <inheritdoc />
	public string Name => CommentName;
	/// <inheritdoc />
	public string? Value { get; }

	/// <inheritdoc cref="XmlBuilder.Comment"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Comment"/> method instead of this constructor</b>
	/// </remarks>
	public XmlComment(string? value = null)
	{
		Value = value;
	}

	/// <inheritdoc cref="IXmlComment"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public XmlComment(in ReadOnlySpan<char> text, ref int offset)
	{
		offset.AdjustForToken(XmlCharacterConstants.CommentStart);

		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (offset < text.Length)
		{
			valueEndOffset = offset;
			if (text.HasCharactersAtOffset(in offset, XmlCharacterConstants.CommentEnd))
			{
				offset.AdjustForToken(XmlCharacterConstants.CommentEnd);
				break;
			}
                
			offset++;
		}

		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
	}

	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		const char spacer = ' ';

		return stringBuilder
			.Append(XmlCharacterConstants.CommentStart)
			.Append(spacer)
			.Append(Value)
			.Append(spacer)
			.Append(XmlCharacterConstants.CommentEnd);
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