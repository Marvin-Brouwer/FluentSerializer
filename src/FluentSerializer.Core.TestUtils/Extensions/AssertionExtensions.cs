using FluentAssertions;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.TestUtils.Assertions;
using System;

namespace FluentSerializer.Core.TestUtils.Extensions
{
    public static class AssertionExtensions
    {
        /// Custom implementation of <see cref="FluentAssertions.Primitives.StringAssertions"/>
        /// to make it easier to work with escape characters.
        /// </summary>
        [CustomAssertion]
        public static AndConstraint<StringAssertions> ShouldBeBinaryEquatableTo(this string assertions, string expectation)
        {
            return new StringAssertions(assertions).BeEquatableTo(expectation);
        }

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
