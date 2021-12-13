using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlComment"/>
[DebuggerDisplay("<!-- {Value, nq} -->")]
public readonly struct XmlComment : IXmlComment
{
	private static readonly int TypeHashCode = typeof(XmlComment).GetHashCode();

	private const string CommentName = "<!-- comment -->";
	public string Name => CommentName;
	public string? Value { get; }

	/// <inheritdoc cref="XmlBuilder.Comment(string)"/>
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
	public XmlComment(ReadOnlySpan<char> text, ref int offset)
	{
		offset += XmlCharacterConstants.CommentStart.Length;

		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (offset < text.Length)
		{
			valueEndOffset = offset;
			if (text.HasStringAtOffset(offset, XmlCharacterConstants.CommentEnd))
			{
				offset += XmlCharacterConstants.CommentEnd.Length;
				break;
			}
                
			offset++;
		}

		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
	}

	public override string ToString()
	{
		var stringBuilder = new StringBuilder();
		stringBuilder = AppendTo(stringBuilder);
		return stringBuilder.ToString();
	}

	public void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, bool writeNull = true, int indent = 0)
	{
		var stringBuilder = stringBuilders.Get();
		writer.Write(AppendTo(stringBuilder, format, indent, writeNull));
		stringBuilders.Return(stringBuilder);
	}

	public StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
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

	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

	#endregion
}