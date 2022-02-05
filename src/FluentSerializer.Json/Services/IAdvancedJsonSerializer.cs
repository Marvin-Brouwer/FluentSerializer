using System;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Services;

/// <summary>
/// The FluentSerializer for JSON with overloads for more advance use-cases
/// </summary>
public interface IAdvancedJsonSerializer : IJsonSerializer
{
	/// <summary>
	/// Serialize <paramref name="model"/> to a node representation
	/// </summary>
	TContainer? SerializeToContainer<TContainer>(object? model, Type modelType)
		where TContainer : IJsonContainer;

	/// <summary>
	/// Deserialize <paramref name="element"/> from a node representation to an object
	/// </summary>
	object? Deserialize(IJsonContainer element, Type modelType);
}