using System.Diagnostics;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System;
using System.IO;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.Configuration;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlAttribute"/>
[DebuggerDisplay("{Name,nq}={Value}")]
public readonly struct XmlAttribute : IXmlAttribute
{
	private static readonly int TypeHashCode = typeof(XmlAttribute).GetHashCode();

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

	/// <inheritdoc cref="IXmlAttribute"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public XmlAttribute(in ReadOnlySpan<char> text, ref int offset)
	{
		var nameStartOffset = offset;
		var nameEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagTerminationCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagEndCharacter)) break;
			nameEndOffset = offset;

			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.PropertyAssignmentCharacter)) break;
			offset++;
		}

		Name = text[nameStartOffset..nameEndOffset].ToString().Trim();

		offset.AdjustForToken(XmlCharacterConstants.PropertyAssignmentCharacter);
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagTerminationCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.PropertyWrapCharacter))
			{
				offset.AdjustForToken(XmlCharacterConstants.PropertyWrapCharacter);
				break;
			}
			if (!text.HasWhitespaceAtOffset(in offset)) break;
			offset++;
		}
            
		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagTerminationCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter)) break;
			valueEndOffset = offset;

			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.PropertyWrapCharacter)) break;
			offset++;
		}
            
		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagTerminationCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.PropertyWrapCharacter))
			{
				offset.AdjustForToken(XmlCharacterConstants.PropertyWrapCharacter);
				break;
			}
			if (text.HasWhitespaceAtOffset(in offset)) continue;
			throw new InvalidDataException("Attribute incorrectly terminated");
		}
	}

	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		Guard.Against.NullOrWhiteSpace(Name, nameof(Name), "The attribute was is an illegal state, it contains no Name");

		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		stringBuilder
			.Append(Name)
			.Append(XmlCharacterConstants.PropertyAssignmentCharacter)
			.Append(XmlCharacterConstants.PropertyWrapCharacter);

		if (Value is not null) stringBuilder = stringBuilder.Append(Value);

		stringBuilder
			.Append(XmlCharacterConstants.PropertyWrapCharacter);

		return stringBuilder;
	}

	#region IEquatable

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Name, Value);

	/// <inheritdoc />
	public static bool operator ==(XmlAttribute left, IDataNode right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(XmlAttribute left, IDataNode right) => !left.Equals(right);

	/// <inheritdoc />
	public static bool operator ==(IDataNode left, XmlAttribute right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(IDataNode left, XmlAttribute right) => !left.Equals(right);

	#endregion
}