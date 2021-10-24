using Ardalis.GuardClauses;
using FluentSerializer.Json.DataNodes.Nodes;
using System;
using System.Text;

namespace FluentSerializer.Json.DataNodes
{
    // todo inherit doc of interfaces and write docs for interfaces
    public readonly struct JsonParser
    {
        public static IJsonObject Parse(string value, StringBuilder stringBuilder)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));

            return Parse(value.AsSpan(), stringBuilder);
        }

        public static IJsonObject Parse(ReadOnlySpan<char> value, StringBuilder stringBuilder)
        {
            Guard.Against.Zero(value.Length, nameof(value));
            Guard.Against.InvalidInput(value.IsEmpty, nameof(value), isEmpty => !isEmpty);

            var offset = 0;
            return new JsonObject(value, stringBuilder, ref offset);
        }
    }
}
