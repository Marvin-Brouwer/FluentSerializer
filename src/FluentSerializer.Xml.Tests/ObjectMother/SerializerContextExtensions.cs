using FluentSerializer.Core.Context;
using FluentSerializer.Core.TestUtils.ObjectMother;
using FluentSerializer.Xml.DataNodes;
using Moq;

namespace FluentSerializer.Xml.Tests.ObjectMother;

internal static class SerializerContextExtensions
{
	/// <summary>
	/// Make the <see cref="ISerializerContext.PropertyType"/> return type of <typeparamref name="TProperty"/>
	/// </summary>
	internal static Mock<ISerializerContext<IXmlNode>> WithPropertyType<TProperty>(this Mock<ISerializerContext<IXmlNode>> contextMock) =>
		contextMock.WithPropertyType(typeof(TProperty));
}