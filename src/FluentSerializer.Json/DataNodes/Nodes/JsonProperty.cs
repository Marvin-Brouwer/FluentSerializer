using System;
using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentSerializer.Core.Text;

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

	/// <inheritdoc />
	public string Name { get; }

	private readonly IJsonNode[] _children;

	/// <inheritdoc />
	public bool HasValue { get; }

	/// <inheritdoc />
	public IReadOnlyList<IJsonNode> Children => _children;

	/// <inheritdoc />
	public IJsonNode? Value => _children.FirstOrDefault();

	/// <inheritdoc cref="JsonBuilder.Property(string, IJsonPropertyContent)"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Property(string, IJsonPropertyContent)"/> method instead of this constructor</b>
	/// </remarks>
	public JsonProperty(string name, IJsonPropertyContent? value)
	{
		Guard.Against.InvalidName(name);

		Name = name;
		HasValue = value is not IJsonValue jsonValue || jsonValue.HasValue;

		_children = value is null ? Array.Empty<IJsonNode>() : new IJsonNode[] { value };
	}

	/// <inheritdoc cref="IJsonObject"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonProperty(in ReadOnlySpan<char> text, ref int offset)
	{
		HasValue = false;
		offset.AdjustForToken(JsonCharacterConstants.PropertyWrapCharacter);

		var nameStartOffset = offset;
		var nameEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter)) break;

			offset++;
			nameEndOffset = offset;
		}

		Name = text[nameStartOffset..nameEndOffset].ToString().Trim();

		while (text.WithinCapacity(in offset))
		{
			offset++;

			// Just pretend it's null if no value has been provided
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter))
			{
				_children = Array.Empty<IJsonNode>();
				offset++;
				return;
			}
			if (text.HasWhitespaceAtOffset(in offset)) continue;

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyAssignmentCharacter)) break;
		}

		while (text.WithinCapacity(in offset))
		{
			offset++;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectStartCharacter))
			{
				_children = new IJsonNode[]{
					new JsonObject(in text, ref offset)
				};
				HasValue = true;
				return;
			}
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayStartCharacter))
			{
				_children = new IJsonNode[] {
					new JsonArray(in text, ref offset)
				};
				HasValue = true;
				return;
			}

			if (!text.HasWhitespaceAtOffset(in offset)) break;
			// Just pretend it's null if no value has been provided
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter))
			{
				_children = Array.Empty<IJsonNode>();
				offset++;
				return;
			}
		}

		var jsonValue = new JsonValue(in text, ref offset);
		_children = new IJsonNode[]
		{
			jsonValue
		};
		HasValue = jsonValue.HasValue;
	}

	/// <inheritdoc />
	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	/// <inheritdoc />
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
		else stringBuilder.AppendNode(childValue, in format, in indent, in writeNull);

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
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Name, _children);

	#endregion
}