using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.DataNodes.Nodes;
using System.Collections.Generic;

namespace FluentSerializer.Json
{
    // todo inherit doc of interfaces and write docs for interfaces
    public readonly struct JsonBuilder
    {
        public static IJsonObject Object(params IJsonProperty[] properties) => new JsonObject(properties);
        public static IJsonObject Object(IEnumerable<IJsonProperty> properties) => new JsonObject(properties);
        public static IJsonArray Array(params IJsonContainer[] elements) => new JsonArray(elements);
        public static IJsonArray Array(IEnumerable<IJsonContainer> elements) => new JsonArray(elements);

        public static IJsonProperty Property(string name, IJsonArray jsonArray)
        {
            Guard.Against.InvalidName(name, nameof(name));

            return new JsonProperty(name, jsonArray);
        }

        public static IJsonProperty Property(string name, IJsonObject jsonObject)
        {
            Guard.Against.InvalidName(name, nameof(name));

            return new JsonProperty(name, jsonObject);
        }

        public static IJsonProperty Property(string name, IJsonValue jsonValue)
        {
            Guard.Against.InvalidName(name, nameof(name));

            return new JsonProperty(name, jsonValue);
        }

        public static IJsonValue Value(string? value)
        {
            return new JsonValue(value);
        }
    }
}
