using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonObject"/>
[DebuggerDisplay("{ObjectName, nq}")]
public readonly struct JsonObject : IJsonObject
{
	private static readonly int TypeHashCode = typeof(JsonObject).GetHashCode();

	private const string ObjectName = "{ }";
	public string Name => ObjectName;
        
	private readonly ulong? _lastPropertyIndex;
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

	/// <inheritdoc cref="JsonBuilder.Object(IEnumerable{IJsonObjectContent}))"/>
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
			var currentPropertyIndex = 0uL;
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
	public JsonObject(ReadOnlySpan<char> text, ref int offset)
	{
		_children = new List<IJsonNode>();
		_lastPropertyIndex = null;
		var currentPropertyIndex = 0uL;

		offset++;
		while (offset < text.Length)
		{
			var character = text[offset];
                
			if (character == JsonCharacterConstants.ObjectStartCharacter) break;
			if (character == JsonCharacterConstants.ArrayStartCharacter) break;
			if (character == JsonCharacterConstants.ObjectEndCharacter) break;
			if (character == JsonCharacterConstants.ArrayEndCharacter) break;

			if (text.HasStringAtOffset(offset, JsonCharacterConstants.SingleLineCommentMarker))
			{
				_children.Add(new JsonCommentSingleLine(text, ref offset));
				currentPropertyIndex++;
				continue;
			}
			if (text.HasStringAtOffset(offset, JsonCharacterConstants.MultiLineCommentStart))
			{
				_children.Add(new JsonCommentMultiLine(text, ref offset));
				currentPropertyIndex++;
				continue;
			}

			offset++;
			if (character == JsonCharacterConstants.PropertyWrapCharacter)
			{
				var jsonProperty = new JsonProperty(text, ref offset);
				_children.Add(jsonProperty);
				_lastPropertyIndex = currentPropertyIndex;
				currentPropertyIndex++;
			}
		}
		offset++;
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

		stringBuilder = AppendTo(stringBuilder, format, indent, writeNull);
		writer.Write(stringBuilder);

		stringBuilder.Clear();
		stringBuilders.Return(stringBuilder);
	}

	public StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
	{
		var childIndent = indent + 1;
		var currentPropertyIndex = 0uL;

		stringBuilder
			.Append(JsonCharacterConstants.ObjectStartCharacter);

		foreach (var child in Children)
		{
			if (!writeNull && child is IJsonProperty jsonProperty && !jsonProperty.HasValue) continue;

			stringBuilder
				.AppendOptionalNewline(format)
				.AppendOptionalIndent(childIndent, format)
				.AppendNode(child, format, childIndent, writeNull);
                
			// Make sure the last item does not append a comma to confirm to JSON spec.
			if (child is not IJsonComment && !currentPropertyIndex.Equals(_lastPropertyIndex)) 
				stringBuilder.Append(JsonCharacterConstants.DividerCharacter);

			currentPropertyIndex++;
		}

		stringBuilder
			.AppendOptionalNewline(format)
			.AppendOptionalIndent(indent, format)
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