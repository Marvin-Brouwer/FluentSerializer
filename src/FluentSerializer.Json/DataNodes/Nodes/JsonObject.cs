using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonObject"/>
[DebuggerDisplay("{ObjectName, nq}")]
public readonly struct JsonObject : IJsonObject
{
	private static readonly int TypeHashCode = typeof(JsonObject).GetHashCode();

	private const string ObjectName = "{ }";
	public string Name => ObjectName;
        
	private readonly int? _lastPropertyIndex;
	private readonly List<IJsonNode> _children;
	public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

	public IJsonProperty? GetProperty(string name)
	{
		Guard.Against.InvalidName(name, nameof(name));

		return _children.FirstOrDefault(child => 
			child.Name.Equals(name, StringComparison.Ordinal)) as IJsonProperty;
	}

	/// <inheritdoc cref="JsonBuilder.Object(IJsonObjectContent[])"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Object"/> method instead of this constructor</b>
	/// </remarks>
	public JsonObject(params IJsonObjectContent[] properties) : this(properties.AsEnumerable()) { }

	/// <inheritdoc cref="JsonBuilder.Object(IEnumerable{IJsonObjectContent})"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Object"/> method instead of this constructor</b>
	/// </remarks>
	public JsonObject(IEnumerable<IJsonObjectContent>? properties)
	{
		_lastPropertyIndex = null;

		if (properties is null)
		{
			_children = new List<IJsonNode>(0);
		}
		else
		{
			var currentPropertyIndex = 0;
			_children = new List<IJsonNode>();
			foreach (var property in properties)
			{
				_children.Add(property);
				if (property is not IJsonComment) _lastPropertyIndex = currentPropertyIndex;
				currentPropertyIndex++;
			}
		}
	}

	/// <inheritdoc cref="IJsonObject"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonObject(in ReadOnlySpan<char> text, ref int offset)
	{
		_children = new List<IJsonNode>();
		_lastPropertyIndex = null;
		var currentPropertyIndex = 0;

		offset.AdjustForToken(JsonCharacterConstants.ObjectStartCharacter);
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectEndCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayEndCharacter)) break;

			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.SingleLineCommentMarker))
			{
				_children.Add(new JsonCommentSingleLine(in text, ref offset));
				currentPropertyIndex++;
				continue;
			}
			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.MultiLineCommentStart))
			{
				_children.Add(new JsonCommentMultiLine(in text, ref offset));
				currentPropertyIndex++;
				continue;
			}
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter))
			{
				var jsonProperty = new JsonProperty(in text, ref offset);
				_children.Add(jsonProperty);
				_lastPropertyIndex = currentPropertyIndex;
				currentPropertyIndex++;
			}

			offset++;
		}
		offset.AdjustForToken(JsonCharacterConstants.ObjectEndCharacter);
	}

	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		var childIndent = indent + 1;
		var currentPropertyIndex = 0;

		stringBuilder
			.Append(JsonCharacterConstants.ObjectStartCharacter);

		foreach (var child in Children)
		{
			if (!writeNull && child is IJsonProperty jsonProperty && !jsonProperty.HasValue) continue;

			stringBuilder
				.AppendOptionalNewline(in format)
				.AppendOptionalIndent(in childIndent, in format)
				.AppendNode(child, in format, in childIndent, in writeNull);
                
			// Make sure the last item does not append a comma to confirm to JSON spec.
			if (child is not IJsonComment && !currentPropertyIndex.Equals(_lastPropertyIndex))
				stringBuilder.Append(JsonCharacterConstants.DividerCharacter);

			currentPropertyIndex++;
		}

		stringBuilder
			.AppendOptionalNewline(in format)
			.AppendOptionalIndent(indent, in format)
			.Append(JsonCharacterConstants.ObjectEndCharacter);

		return stringBuilder;
	}

	#region IEquatable

	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, _children);

	#endregion
}