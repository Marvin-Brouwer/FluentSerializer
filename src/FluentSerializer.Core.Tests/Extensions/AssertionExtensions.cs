using FluentAssertions;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Tests.Assertions;
using System;

namespace FluentSerializer.Core.Tests.Extensions
{
    public static class AssertionExtensions
    {
        /// <summary>
        /// Assert objects are equal using a specific implementation of <see cref="IEquatable{T}"/>.
        /// This is especially useful for records where you try to override the equality compare,
        /// by default FluentAssertions will not understand that.
        /// </summary>
        [CustomAssertion]
        public static EquatableAssertions<TEquatable> Should<TEquatable>(this TEquatable assertions)
            where TEquatable : IEquatable<TEquatable>
        {
            return new EquatableAssertions<TEquatable>(assertions);
        }

        /// <inheritdoc cref="FluentAssertions.AssertionExtensions.Should{T}(System.IComparable{T})"/>
        [CustomAssertion]
        public static EquatableAssertions<IDataNode> Should(this IDataNode assertions)
        {
            return new EquatableAssertions<IDataNode>(assertions);
        }
    }
}
