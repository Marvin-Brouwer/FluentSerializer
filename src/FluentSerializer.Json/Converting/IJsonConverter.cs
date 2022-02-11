using FluentSerializer.Core.Converting;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Converting;

/// <inheritdoc />
public interface IJsonConverter : IConverter<IJsonNode, IJsonNode>
{
}