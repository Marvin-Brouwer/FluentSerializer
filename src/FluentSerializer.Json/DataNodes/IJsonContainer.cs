using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Json.DataNodes;

/// <summary>
/// An implementation of <see cref="IDataContainer{TValue}"/> for JSON
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Warning", "S2326:'TContainer' is not used in the interface",
	Justification = "This type parameter is meant for constraining this interface to the type constraint", Scope = "type", Target = "~T:FluentSerializer.Json.DataNodes.IJsonContainer`1")]
public interface IJsonContainer<out TContainer> : IJsonContainer
	where TContainer : IDataContainer<IJsonNode>
{ }

/// <inheritdoc cref="IJsonContainer{TContainer}"/>
public interface IJsonContainer : IDataContainer<IJsonNode>, IJsonNode
{ }