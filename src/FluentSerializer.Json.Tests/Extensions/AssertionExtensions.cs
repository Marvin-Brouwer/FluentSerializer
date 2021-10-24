using FluentAssertions;
using FluentSerializer.Core.Tests.Assertions;
using FluentSerializer.Json.DataNodes;
using System;
using System.Collections.Generic;
using System.Text;

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
