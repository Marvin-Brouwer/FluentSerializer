using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentSerializer.Core.Text;
using FluentSerializer.Core.Text.Extensions;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonArray"/>
[DebuggerDisplay("{ArrayName, nq}")]
public readonly struct JsonArray : IJsonArray
{
	private static readonly int TypeHashCode = typeof(JsonArray).GetHashCode();

	private const string ArrayName = "[ ]";
	public string Name => ArrayName;

	private readonly ulong? _lastNonCommentChildIndex;
	private readonly List<IJsonNode> _children;
	public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

	/// <inheritdoc cref="JsonBuilder.Array(IJsonArrayContent[])"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Array"/> method instead of this constructor</b>
	/// </remarks>
	public JsonArray(params IJsonArrayContent[] elements) : this(elements.AsEnumerable()) { }

	/// <inheritdoc cref="JsonBuilder.Array(IEnumerable{IJsonArrayContent})"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Array"/> method instead of this constructor</b>
	/// </remarks>
	public JsonArray(IEnumerable<IJsonArrayContent>? elements)
	{
		_lastNonCommentChildIndex = null;

		if (elements is null)
		{
			_children = new List<IJsonNode>(0);
		}
		else
		{
			_children = new List<IJsonNode>();
			var currentChildIndex = 0uL;
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
	public JsonArray(in ITokenReader reader)
	{
		_children = new List<IJsonNode>();
		_lastNonCommentChildIndex = null;
		var currentChildIndex = 0uL;

		reader.Advance();

		while (reader.CanAdvance())
		{
			if (reader.HasCharacterAtOffset(JsonCharacterConstants.ObjectStartCharacter))
			{
				var jsonObject = new JsonObject(reader);
				_children.Add(jsonObject);
				_lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}
			if (reader.HasCharacterAtOffset(JsonCharacterConstants.ArrayStartCharacter))
			{
				var jsonArray = new JsonArray(reader);
				_children.Add(jsonArray);
				_lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}

			if (reader.HasCharacterAtOffset(JsonCharacterConstants.PropertyWrapCharacter)) break;
			if (reader.HasCharacterAtOffset(JsonCharacterConstants.ObjectEndCharacter)) break;
			if (reader.HasCharacterAtOffset(JsonCharacterConstants.ArrayEndCharacter)) break;

			if (reader.HasStringAtOffset(JsonCharacterConstants.SingleLineCommentMarker))
			{
				_children.Add(new JsonCommentSingleLine(reader));

				currentChildIndex++;
				continue;
			}
			if (reader.HasStringAtOffset(JsonCharacterConstants.MultiLineCommentStart))
			{
				_children.Add(new JsonCommentMultiLine(reader));

				currentChildIndex++;
				continue;
			}
			reader.Advance();
		}
		if (reader.CanAdvance()) reader.Advance();
	}

	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		var childIndent = indent + 1;
		var currentChildIndex = 0uL;

		stringBuilder
			.Append(JsonCharacterConstants.ArrayStartCharacter);
            
		foreach (var child in Children)
		{
			stringBuilder
				.AppendOptionalNewline(format)
				.AppendOptionalIndent(childIndent, format)
				.AppendNode(child, format, childIndent, writeNull);

			// Make sure the last item does not append a comma to confirm to JSON spec.
			if (child is not IJsonComment && !currentChildIndex.Equals(_lastNonCommentChildIndex))
				stringBuilder.Append(JsonCharacterConstants.DividerCharacter);

			currentChildIndex++;
		}

		stringBuilder
			.AppendOptionalNewline(format)
			.AppendOptionalIndent(indent, format)
			.Append(JsonCharacterConstants.ArrayEndCharacter);

		return stringBuilder;
	}

	#region IEquatable

	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, _children);

	#endregion
}