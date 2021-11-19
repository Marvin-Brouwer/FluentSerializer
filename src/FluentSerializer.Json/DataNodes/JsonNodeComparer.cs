using System;
using System.Collections.Generic;

namespace FluentSerializer.Json.DataNodes
{
    /// <summary>
    /// Simple comparer between <see cref="IJsonNode"/>s, relying on <see cref="IEquatable{IJsonNode?}.GetHashCode()"/>
    /// </summary>
    internal readonly struct JsonNodeComparer : IEqualityComparer<IEquatable<IJsonNode>>
    {
        internal static readonly JsonNodeComparer Default = new();

        public bool Equals(IEquatable<IJsonNode>? x, IEquatable<IJsonNode>? y) => x?.GetHashCode().Equals(y?.GetHashCode()) == true;

        public int GetHashCode(IEquatable<IJsonNode> obj) => obj.GetHashCode();
    }
}
