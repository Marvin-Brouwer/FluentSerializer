using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;

namespace FluentSerializer.Core.Tests.Assertions
{
    /// <inheritdoc cref="AssertionExtensions.Should{T}(IComparable{T})"/>
    public class StringAssertions : ReferenceTypeAssertions<string, StringAssertions>
    {
        public StringAssertions(string instance)
            : base(instance)
        {
        }

        protected override string Identifier => Subject;

        public AndConstraint<StringAssertions> BeEquatableTo(string expectation)
        {
            Execute.Assertion
                .Given(() => Subject.Equals(expectation, StringComparison.Ordinal))
                .ForCondition(result => result)
                .FailWith("Expected result to be \n{0}, but found \n{1}",
                    _ => ReplaceEscapeCharacters(expectation), _ => ReplaceEscapeCharacters(Subject));

            return new AndConstraint<StringAssertions>(this);
        }

        private static string ReplaceEscapeCharacters(string input) =>
            input.Replace("\n", "\n\\n ").Replace("\r", "\r\\r ")
				.Replace("\r\\r\n\\n", "\n\\r\\n ").Replace("\t", "\\t ");
    }
}
