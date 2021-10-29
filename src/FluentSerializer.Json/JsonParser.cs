using Ardalis.GuardClauses;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.DataNodes.Nodes;
using System;

namespace FluentSerializer.Json
{
    // todo inherit doc of interfaces and write docs for interfaces
    public readonly struct JsonParser
    {
        public static IJsonObject Parse(string value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));

            return Parse(value.AsSpan());
        }

        public static IJsonObject Parse(ReadOnlySpan<char> value)
        {
            Guard.Against.Zero(value.Length, nameof(value));
            Guard.Against.InvalidInput(value.IsEmpty, nameof(value), isEmpty => !isEmpty);

            var offset = 0;
            return new JsonObject(value, ref offset);
        }
    }
}
