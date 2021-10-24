using FluentAssertions;
using FluentSerializer.Core.Tests.Assertions;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Tests.Extensions
{
    public static class AssertionExtensions
    {
        /// <inheritdoc cref="AssertionExtensions.Should{T}(IComparable{T})"/>
        [CustomAssertion]
        public static EquatableAssertions<IJsonNode> Should(this IJsonNode assertions)
        {
            return new EquatableAssertions<IJsonNode>(assertions);
        }
    }
}
