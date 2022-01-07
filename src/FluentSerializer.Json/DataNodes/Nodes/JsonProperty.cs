using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonProperty"/>
[DebuggerDisplay("{Name}: {GetDebugValue(), nq},")]
public readonly struct JsonProperty : IJsonProperty
{
	private static readonly int TypeHashCode = typeof(JsonProperty).GetHashCode();

	[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
	private string GetDebugValue()
	{
		if (_children.Length == 0) return JsonCharacterConstants.NullValue;
		var value = _children[0];
		if (value is JsonValue jsonValue) return jsonValue.Value ?? JsonCharacterConstants.NullValue;
		return value.Name;
	}

	public string Name { get; }

	private readonly IJsonNode[] _children;

	public bool HasValue { get; }

	public IReadOnlyList<IJsonNode> Children => _children;

	public IJsonNode? Value => _children.FirstOrDefault();

	/// <inheritdoc cref="JsonBuilder.Property(string, IJsonPropertyContent)"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Property"/> method instead of this constructor</b>
	/// </remarks>
	public JsonProperty(string name, IJsonPropertyContent value)
	{
		Guard.Against.InvalidName(name, nameof(name));

		Name = name;
		HasValue = value is not IJsonValue jsonValue || jsonValue.HasValue;

		_children = new IJsonNode[] { value };
	}

	/// <inheritdoc cref="IJsonObject"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonProperty(ReadOnlySpan<char> text, ref int offset)
	{
		HasValue = false;

		var nameStartOffset = offset;
		var nameEndOffset = offset;

		while (offset < text.Length)
		{
			nameEndOffset = offset;

			var character = text[offset];

			if (character == JsonCharacterConstants.ObjectEndCharacter) break;
			if (character == JsonCharacterConstants.ArrayEndCharacter) break;
			offset++;
			if (character == JsonCharacterConstants.DividerCharacter) break;
			if (character == JsonCharacterConstants.PropertyWrapCharacter) break;
		}
            
		Name = text[nameStartOffset..nameEndOffset].ToString().Trim();

		while (offset < text.Length)
		{
			var character = text[offset];

			if (character == JsonCharacterConstants.ObjectEndCharacter) break;
			if (character == JsonCharacterConstants.ArrayEndCharacter) break;
			offset++;
			if (character == JsonCharacterConstants.DividerCharacter) break;
			if (char.IsWhiteSpace(character)) continue;

			if (character == JsonCharacterConstants.PropertyAssignmentCharacter) break;
		}

		_children = new IJsonNode[1];

		while (offset < text.Length)
		{
			var character = text[offset];

			if (character == JsonCharacterConstants.ObjectEndCharacter) return;
			if (character == JsonCharacterConstants.ArrayEndCharacter) return;

			if (character == JsonCharacterConstants.ObjectStartCharacter)
			{
				_children[0] = new JsonObject(text, ref offset);
				HasValue = true;
				return;
			}
			if (character == JsonCharacterConstants.ArrayStartCharacter)
			{
				_children[0] = new JsonArray(text, ref offset);
				HasValue = true;
				return;
			}

			if (!char.IsWhiteSpace(character)) break;
			offset++;
			if (character == JsonCharacterConstants.DividerCharacter) return;
		}

		var jsonValue = new JsonValue(text, ref offset);
		_children[0] = jsonValue;
		HasValue = jsonValue.HasValue;
	}

	public override string ToString() => ((IDataNode)this).ToString(JsonSerializerConfiguration.Default);

	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		Guard.Against.NullOrWhiteSpace(Name, nameof(Name), "The property was is an illegal state, it contains no Name");

		const char spacer = ' ';

		if (!writeNull && !HasValue) return stringBuilder;

		var childValue = Children.FirstOrDefault();

		stringBuilder
			.Append(JsonCharacterConstants.PropertyWrapCharacter)
			.Append(Name)
			.Append(JsonCharacterConstants.PropertyWrapCharacter);

		stringBuilder.Append(JsonCharacterConstants.PropertyAssignmentCharacter);
		if (format) stringBuilder.Append(spacer);

		if (childValue is null) stringBuilder.Append(JsonCharacterConstants.NullValue);
		else stringBuilder.AppendNode(childValue, format, indent, writeNull);

		return stringBuilder;
	}


	#region IEquatable

	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Name, _children);

	#endregion
}