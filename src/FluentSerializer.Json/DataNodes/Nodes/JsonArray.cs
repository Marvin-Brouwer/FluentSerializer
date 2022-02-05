using System;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonArray"/>
[DebuggerDisplay("{ArrayName, nq}")]
public readonly struct JsonArray : IJsonArray
{
	private static readonly int TypeHashCode = typeof(JsonArray).GetHashCode();

	private const string ArrayName = "[ ]";
	/// <inheritdoc />
	public string Name => ArrayName;

	private readonly int? _lastNonCommentChildIndex;
	private readonly List<IJsonNode> _children;
	/// <inheritdoc />
	public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

	/// <inheritdoc cref="JsonBuilder.Array(in IEnumerable{IJsonArrayContent})"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Array(in IEnumerable{IJsonArrayContent})"/> method instead of this constructor</b>
	/// </remarks>
	public JsonArray(in IEnumerable<IJsonArrayContent>? elements)
	{
		_lastNonCommentChildIndex = null;

		if (elements is null)
		{
			_children = new List<IJsonNode>(0);
		}
		else
		{
			_children = new List<IJsonNode>();
			var currentChildIndex = 0;
			foreach (var property in elements)
			{
				_children.Add(property);
				if (property is not IJsonComment) _lastNonCommentChildIndex = currentChildIndex;
				currentChildIndex++;

			}
		}
	}

	/// <inheritdoc cref="IJsonArray"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonArray(in ReadOnlySpan<char> text, ref int offset)
	{
		_children = new List<IJsonNode>();
		_lastNonCommentChildIndex = null;
		var currentChildIndex = 0;

		offset.AdjustForToken(JsonCharacterConstants.ArrayStartCharacter);
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectStartCharacter))
			{
				_children.Add(new JsonObject(in text, ref offset));
				_lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayStartCharacter))
			{
				_children.Add(new JsonArray(in text, ref offset));
				_lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectEndCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayEndCharacter)) break;

			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.SingleLineCommentMarker))
			{
				_children.Add(new JsonCommentSingleLine(in text, ref offset));

				currentChildIndex++;
				continue;
			}
			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.MultiLineCommentStart))
			{
				_children.Add(new JsonCommentMultiLine(in text, ref offset));

				currentChildIndex++;
				continue;
			}
			offset++;
		}
		offset.AdjustForToken(JsonCharacterConstants.ArrayEndCharacter);
	}

	/// <inheritdoc />
	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		var childIndent = indent + 1;
		var currentChildIndex = 0;

		stringBuilder
			.Append(JsonCharacterConstants.ArrayStartCharacter);
            
		foreach (var child in Children)
		{
			stringBuilder
				.AppendOptionalNewline(in format)
				.AppendOptionalIndent(in childIndent, in format)
				.AppendNode(child, in format, in childIndent, in writeNull);

			// Make sure the last item does not append a comma to confirm to JSON spec.
			if (child is not IJsonComment && !currentChildIndex.Equals(_lastNonCommentChildIndex))
				stringBuilder.Append(JsonCharacterConstants.DividerCharacter);

			currentChildIndex++;
		}

		stringBuilder
			.AppendOptionalNewline(in format)
			.AppendOptionalIndent(in indent, format)
			.Append(JsonCharacterConstants.ArrayEndCharacter);

		return stringBuilder;
	}

	#region IEquatable

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, _children);

	#endregion
}