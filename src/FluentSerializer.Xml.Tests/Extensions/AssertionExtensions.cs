using FluentAssertions;
using FluentSerializer.Core.Tests.Assertions;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Tests.Extensions
{
    public static class AssertionExtensions
    {
        /// <inheritdoc cref="AssertionExtensions.Should{T}(IComparable{T})"/>
        [CustomAssertion]
        public static EquatableAssertions<IXmlNode> Should(this IXmlNode assertions)
        {
            return new EquatableAssertions<IXmlNode>(assertions);
        }
    }
}
