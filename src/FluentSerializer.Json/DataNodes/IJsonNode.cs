using FluentSerializer.Core.DataNodes;
using System;

namespace FluentSerializer.Json.DataNodes
{
    public interface IJsonNode : IDataNode, IEquatable<IJsonNode?> { }
}
