using FluentSerializer.Core.Context;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Json.DataNodes;
using Moq;

namespace FluentSerializer.Json.Tests.ObjectMother;

internal static class SerializerContextMother
{
	/// <summary>
	/// Make the <see cref="ISerializerContext.PropertyType"/> return type of <typeparamref name="TProperty"/>
	/// </summary>
	internal static Mock<ISerializerContext<IJsonNode>> WithPropertyType<TProperty>(this Mock<ISerializerContext<IJsonNode>> contextMock) =>
		contextMock.WithPropertyType(typeof(TProperty));
}