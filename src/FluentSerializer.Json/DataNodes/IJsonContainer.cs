using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Json.DataNodes;

/// <summary>
/// An implementation of <see cref="IDataContainer{TValue}"/> for JSON
/// </summary>
public interface IJsonContainer<out TContainer> : IJsonContainer
	where TContainer : IDataContainer<IJsonNode>
{ }

/// <inheritdoc cref="IJsonContainer{TContainer}"/>
public interface IJsonContainer : IDataContainer<IJsonNode>, IJsonNode
{ }