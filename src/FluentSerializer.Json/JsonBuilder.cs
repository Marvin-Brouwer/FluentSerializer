using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.DataNodes.Nodes;
using System.Collections.Generic;

namespace FluentSerializer.Json;

/// <summary>
/// JSON object builder utility class
/// </summary>
public readonly struct JsonBuilder
{
	/// <inheritdoc cref="IJsonObject"/>
	/// <param name="properties">A parameters list of <see cref="IJsonObjectContent"/> as children of this object node.</param>
	public static IJsonObject Object(params IJsonObjectContent[] properties) => new JsonObject(properties);
	/// <inheritdoc cref="IJsonObject"/>
	/// <param name="properties">A collection of <see cref="IJsonObjectContent"/> as children of this object node.</param>
	public static IJsonObject Object(IEnumerable<IJsonObjectContent> properties) => new JsonObject(properties);

	/// <inheritdoc cref="IJsonArray"/>
	/// <param name="elements">A parameters list of <see cref="IJsonArrayContent"/> as children of this array node.</param>
	public static IJsonArray Array(params IJsonArrayContent[] elements) => new JsonArray(elements);
	/// <inheritdoc cref="IJsonArray"/>
	/// <param name="elements">A collection of <see cref="IJsonArrayContent"/> as children of this array node.</param>
	public static IJsonArray Array(IEnumerable<IJsonArrayContent> elements) => new JsonArray(elements);

	/// <inheritdoc cref="Property(string, IJsonPropertyContent)"/>
	/// <param name="name">A valid property name, will throw if given anything other than word characters.</param>
	/// <param name="jsonArray">The array node assigned to this property's value</param>
	public static IJsonProperty Property(string name, IJsonArray? jsonArray)
	{
		Guard.Against.InvalidName(name, nameof(name));

		return new JsonProperty(name, jsonArray);
	}
	/// <inheritdoc cref="Property(string, IJsonPropertyContent)"/>
	/// <param name="name">A valid property name, will throw if given anything other than word characters.</param>
	/// <param name="jsonObject">The object node assigned to this property's value</param>
	public static IJsonProperty Property(string name, IJsonObject? jsonObject)
	{
		Guard.Against.InvalidName(name, nameof(name));

		return new JsonProperty(name, jsonObject);
	}
	/// <inheritdoc cref="Property(string, IJsonPropertyContent)"/>
	/// <param name="name">A valid property name, will throw if given anything other than word characters.</param>
	/// <param name="jsonValue">The value container assigned to this property's value</param>
	public static IJsonProperty Property(string name, IJsonValue? jsonValue)
	{
		Guard.Against.InvalidName(name, nameof(name));

		return new JsonProperty(name, jsonValue);
	}
	/// <inheritdoc cref="IJsonProperty"/>
	/// <param name="name">A valid property name, will throw if given anything other than word characters.</param>
	/// <param name="jsonPropertyItem">The node that carries the value of this property</param>
	public static IJsonProperty Property(string name, IJsonPropertyContent jsonPropertyItem)
	{
		Guard.Against.InvalidName(name, nameof(name));

		return new JsonProperty(name, jsonPropertyItem);
	}

	/// <inheritdoc cref="IJsonValue"/>
	/// <param name="value">The raw string representation of this properties value</param>
	public static IJsonValue Value(string? value) => new JsonValue(value);

	/// <inheritdoc cref="IJsonComment"/>
	/// <param name="value">The text to display as a comment</param>
	/// <remarks>
	/// When pretty formatting is turned off this will output as a <see cref="MultilineComment" /> 
	/// because otherwise it would break the document.
	/// </remarks>
	public static IJsonComment Comment(string value)
	{
		Guard.Against.NullOrEmpty(value, nameof(value));

		return new JsonCommentSingleLine(value);
	}

	/// <inheritdoc cref="IJsonComment"/>
	/// <param name="value">The text to display as a comment</param>
	public static IJsonComment MultilineComment(string value)
	{
		Guard.Against.NullOrEmpty(value, nameof(value));

		return new JsonCommentMultiLine(value);
	}
}