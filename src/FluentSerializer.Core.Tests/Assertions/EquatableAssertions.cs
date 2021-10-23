using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;

namespace FluentSerializer.Core.Tests.Assertions
{
    /// <inheritdoc cref="AssertionExtensions.Should{T}(IComparable{T})"/>
    public class EquatableAssertions<TSubjectEquatable> :
       ReferenceTypeAssertions<TSubjectEquatable, EquatableAssertions<TSubjectEquatable>>
        where TSubjectEquatable : IEquatable<TSubjectEquatable>
    {
        public EquatableAssertions(TSubjectEquatable instance)
            : base(instance)
        {
        }

        protected override string Identifier => Subject.ToString();

        public AndConstraint<EquatableAssertions<TSubjectEquatable>> BeEquatableTo<TEquatable>(
            TEquatable expectation, string because = "", params object[] becauseArgs)
            where TEquatable : IEquatable<TEquatable>
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => (Subject is TEquatable target) && target.Equals(expectation))
                .ForCondition(result => result)
                .FailWith("Expected result to be {0}, but found {1}.",
                    _ => Subject.ToString(), _ => expectation.ToString());

            return new AndConstraint<EquatableAssertions<TSubjectEquatable>>(this);
        }
    }
}
