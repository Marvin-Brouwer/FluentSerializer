using FluentSerializer.Core.DataNodes;
using System;

namespace FluentSerializer.Json.DataNodes;

/// <summary>
/// A generic representation of a JSON node
/// </summary>
public interface IJsonNode : IDataNode, IEquatable<IJsonNode?> { }