using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;
using Moq;
using System;

namespace FluentSerializer.Json.Tests.ObjectMother;

internal static class JsonSerializerObjectMother
{
	/// <summary>
	/// Setup <see cref="IAdvancedJsonSerializer.SerializeToNode(in object?, in Type)"/>
	/// to simply return the IJsonObject passed in.
	/// </summary>
	/// <param name="serializerMock"></param>
	/// <returns></returns>
	internal static Mock<IAdvancedJsonSerializer> WithSerializeToContainer(
		this Mock<IAdvancedJsonSerializer> serializerMock)
	{
		serializerMock
			.Setup(serializer => serializer
				.SerializeToContainer<IJsonContainer>(in It.Ref<object?>.IsAny, in It.Ref<Type>.IsAny))
			.Returns((object? element, Type _) => element as IJsonContainer);

		return serializerMock;
	}

	/// <summary>
	/// Setup <see cref="IAdvancedJsonSerializer.Deserialize(in IJsonObject, in Type)"/>
	/// to simply return the IJsonObject passed in.
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