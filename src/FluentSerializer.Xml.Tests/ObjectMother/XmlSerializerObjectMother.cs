using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;
using Moq;
using System;

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
	/// Setup <see cref="IAdvancedXmlSerializer.Deserialize(in IXmlElement, in Type)"/>
	/// to simply return the IXmlElement passed in.
	/// </summary>
	/// <param name="serializerMock"></param>
	/// <returns></returns>
	internal static Mock<IAdvancedXmlSerializer> WithDeserialize(
		this Mock<IAdvancedXmlSerializer> serializerMock)
	{
		serializerMock
			.Setup(serializer => serializer
				.Deserialize(in It.Ref<IXmlElement>.IsAny, in It.Ref<Type>.IsAny))
			.Returns((IXmlElement element, Type _) => element);

		return serializerMock;
	}
}