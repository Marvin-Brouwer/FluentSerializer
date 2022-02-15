using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;
using Moq;
using System;

namespace FluentSerializer.Json.Tests.ObjectMother;

internal static class JsonSerializerObjectMother
{
	/// <summary>
	/// Setup <see cref="IAdvancedJsonSerializer.SerializeToElement(in object?, in Type)"/>
	/// to simply return the IJsonElement passed in.
	/// </summary>
	/// <param name="serializerMock"></param>
	/// <returns></returns>
	internal static Mock<IAdvancedJsonSerializer> WithSerializeToContainer<TContainer>(
		this Mock<IAdvancedJsonSerializer> serializerMock)
		where TContainer: IJsonContainer
	{
		serializerMock
			.Setup(serializer => serializer
				.SerializeToContainer<TContainer>(in It.Ref<object?>.IsAny, in It.Ref<Type>.IsAny))
			.Returns((object? element, Type _) => (TContainer?)element);

		return serializerMock;
	}

	/// <summary>
	/// Setup <see cref="IAdvancedJsonSerializer.Deserialize(in IJsonElement, in Type)"/>
	/// to simply return the IJsonElement passed in.
	/// </summary>
	/// <param name="serializerMock"></param>
	/// <returns></returns>
	internal static Mock<IAdvancedJsonSerializer> WithDeserialize(
		this Mock<IAdvancedJsonSerializer> serializerMock)
	{
		serializerMock
			.Setup(serializer => serializer
				.Deserialize(in It.Ref<IJsonContainer>.IsAny, in It.Ref<Type>.IsAny))
			.Returns((IJsonContainer element, Type _) => element);

		return serializerMock;
	}
}