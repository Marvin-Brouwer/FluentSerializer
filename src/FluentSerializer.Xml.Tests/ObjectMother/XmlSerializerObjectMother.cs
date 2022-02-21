using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;
using Moq;
using System;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Xml.Tests.ObjectMother;

internal static class XmlSerializerObjectMother
{
	/// <summary>
	/// Setup <see cref="IAdvancedXmlSerializer.SerializeToElement(in object?, in Type)"/>
	/// to simply return the IXmlElement passed in.
	/// </summary>
	/// <param name="serializerMock"></param>
	/// <returns></returns>
	internal static Mock<IAdvancedXmlSerializer> WithSerializeToElement(
		this Mock<IAdvancedXmlSerializer> serializerMock)
	{
		serializerMock
			.Setup(serializer => serializer
				.SerializeToElement(in It.Ref<object?>.IsAny, in It.Ref<Type>.IsAny))
			.Returns((object? element, Type _) => element as IXmlElement);

		return serializerMock;
	}

	/// <summary>
	/// Setup <see cref="IAdvancedXmlSerializer.Deserialize(in IXmlElement, in Type, in ISerializerCoreContext{TDataNode})"/>
	/// to simply return the IXmlElement passed in.
	/// </summary>
	/// <param name="serializerMock"></param>
	/// <returns></returns>
	internal static Mock<IAdvancedXmlSerializer> WithDeserialize(
		this Mock<IAdvancedXmlSerializer> serializerMock)
	{
		serializerMock
			.Setup(serializer => serializer
				.Deserialize(in It.Ref<IXmlElement>.IsAny, in It.Ref<Type>.IsAny,
					It.Ref<ISerializerCoreContext<IXmlNode>>.IsAny))
			.Returns((IXmlElement element, Type _, ISerializerCoreContext _) => element);

		return serializerMock;
	}
}