using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;

namespace FluentSerializer.Core.Tests.Assertions
{
    /// <inheritdoc cref="AssertionExtensions.Should{T}(IComparable{T})"/>
    public class EquatableAssertions<TEquatable> :
       ReferenceTypeAssertions<TEquatable, EquatableAssertions<TEquatable>>
        where TEquatable : IEquatable<TEquatable>
    {
        public EquatableAssertions(TEquatable instance)
            : base(instance)
        {
        }

        protected override string Identifier => Subject.ToString();

        public AndConstraint<EquatableAssertions<TEquatable>> BeEquatableTo(
            TEquatable expectation, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => (Subject is TEquatable target) && target.Equals(expectation))
                .ForCondition(result => result)
                .FailWith("Expected result to be {0}, but found {1}.",
                    _ => expectation.ToString(), _ => Subject.ToString());

            return new AndConstraint<EquatableAssertions<TEquatable>>(this);
        }
    }
}
