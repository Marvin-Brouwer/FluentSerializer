using Ardalis.GuardClauses;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.DataNodes.Nodes;
using System;

namespace FluentSerializer.Json
{
    /// <summary>
    /// JSON parsing utility class
    /// </summary>
    public readonly struct JsonParser
    {
        /// <summary>
        /// Parse a string value to a JSON object tree
        /// </summary>
        /// <param name="value">The JSON to parse</param>
        /// <remarks>
        /// This parser will not parse values to C# types, they will all be represented as string.
        /// </remarks>
        public static IJsonObject Parse(string value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));

            return Parse(value.AsSpan());
        }

        /// <inheritdoc cref="Parse(string)"/>
        public static IJsonObject Parse(ReadOnlySpan<char> value)
        {
            Guard.Against.Zero(value.Length, nameof(value));
            Guard.Against.InvalidInput(value.IsEmpty, nameof(value), isEmpty => !isEmpty);

            var offset = 0;
            return new JsonObject(value, ref offset);
        }
    }
}
