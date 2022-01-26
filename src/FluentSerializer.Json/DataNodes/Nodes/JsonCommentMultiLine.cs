using System;
using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System.Diagnostics;
using FluentSerializer.Core.Text;
using FluentSerializer.Core.Text.Extensions;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonComment"/>
[DebuggerDisplay("/* {Value,nq} */")]
public readonly struct JsonCommentMultiLine : IJsonComment
{
	private static readonly int TypeHashCode = typeof(JsonCommentMultiLine).GetHashCode();

	public string Name => JsonCharacterConstants.SingleLineCommentMarker;
	public string? Value { get; }

	/// <inheritdoc cref="JsonBuilder.MultilineComment(string)"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.MultilineComment"/> method instead of this constructor</b>
	/// </remarks>
	public JsonCommentMultiLine(string value)
	{
		Guard.Against.NullOrEmpty(value, nameof(value));

		Value = value;
	}

	/// <inheritdoc cref="IJsonComment"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonCommentMultiLine(in ReadOnlySpan<char> text, ref int offset)
	{
		offset.AdjustForToken(JsonCharacterConstants.MultiLineCommentStart);

		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			valueEndOffset = offset;

			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.MultiLineCommentEnd)) 
			{
				offset.AdjustForToken(JsonCharacterConstants.MultiLineCommentStart);
				break;
			}

			offset++;
		}

		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
	}

	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		// JSON does not support empty property assignment or array members
		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		const char spacer = ' ';

		return stringBuilder
			.Append(JsonCharacterConstants.MultiLineCommentStart)
			.Append(spacer)
			.Append(Value)
			.Append(spacer)
			.Append(JsonCharacterConstants.MultiLineCommentEnd);
	}

	#region IEquatable

	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

	#endregion
}