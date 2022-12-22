using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Exceptions;
using FluentSerializer.Xml.Services;
using FluentSerializer.Xml.Tests.ObjectMother;

using Moq;

using System;
using System.Collections.Generic;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Services;

public sealed class XmlTypeDeserializerTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Deserialize;

	private readonly ISerializerCoreContext<IXmlNode> _coreContextStub;
	private readonly Mock<IClassMap> _classMapMock;
	private readonly Mock<IClassMapCollection> _classMapCollectionMock;

	public XmlTypeDeserializerTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>()
			.UseConfig(XmlSerializerConfiguration.Default);
		_coreContextStub = new SerializerCoreContext<IXmlNode>(serializerMock.Object);
		_classMapMock = new Mock<IClassMap>()
			.WithDefaults()
			.WithoutPropertyMaps();
		_classMapCollectionMock = new Mock<IClassMapCollection>();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Initialize_NullValue_Throws()
	{
		// Act

		var result = () => new XmlTypeDeserializer(null!);
		// Assert
		result.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("classMapCollection");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromElement_NullValues_Throws()
	{
		// Arrange
		var dataObject = Element(nameof(Element));
		var classType = typeof(TestClass);

		var sut = new XmlTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result1 = () => sut.DeserializeFromElement(null!, classType, _coreContextStub);
		var result2 = () => sut.DeserializeFromElement(dataObject, null!, _coreContextStub);
		var result3 = () => sut.DeserializeFromElement(dataObject, classType, null!);
		var result4 = () => sut.DeserializeFromElement<TestClass>(null!, _coreContextStub);
		var result5 = () => sut.DeserializeFromElement<TestClass>(dataObject, null!);

		// Assert
		result1.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(dataObject));
		result2.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(classType));
		result3.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("coreContext");
		result4.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(dataObject))
			.And // This is mostly here to please Stryker
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(5);
		result5.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("coreContext")
			.And // This is mostly here to please Stryker
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(5);
	}

	/// <summary>
	/// We're either parsing a root node or a document.
	/// XML specs that a file cannot have more than one root node.
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromElement_TypeIsEnumerable_Throws()
	{
		// Arrange
		var input = Element("PlaceHolder");

		var type = typeof(IEnumerable<int>);

		_classMapCollectionMock
			.WithoutClassMaps();

		var sut = new XmlTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<MalConfiguredRootNodeException>()
			.Which.AttemptedType.Should().Be(type);
	}

	/// <summary>
	/// We need a profile to deserialize
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromElement_TypeIsNotRegistered_Throws()
	{
		// Arrange
		var input = Element("PlaceHolder");

		var type = typeof(TestClass);

		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ClassMapNotFoundException>()
			.Which.TargetType.Should().Be(type);
	}

	/// <summary>
	/// We need a node to get values
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromElement_NodeIsNotPresent_Throws()
	{
		// Arrange
		var type = typeof(TestClass);

		_classMapMock
			.WithClassType(type);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.DeserializeFromElement(
			Element("IncorrectName"), type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<MissingNodeException>()
			.Which.AttemptedType.Should().Be(type);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromNode_NonNullableValueIsEmpty_Throws()
	{
		// Arrange
		var input = Element(nameof(TestClass));

		var type = typeof(TestClass);
		var containerType = typeof(IXmlElement);
		var property = type.GetProperty(nameof(TestClass.Value));

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, property!);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotFoundException>()
			.Which.TargetType.Should().Be(containerType);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromElement_NoPropertiesMapped_ReturnsEmptyNode()
	{
		// Arrange
		var input = Element(nameof(TestClass));

		var type = typeof(TestClass);

		_classMapMock
			.WithClassType(type);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeOfType(type);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromElement_InvalidPropertyMapping_Throws()
	{
		// Arrange
		var input = Element(nameof(TestClass));
		var type = typeof(TestClass);

		// Any arbitrary type here
		var attemptedContainerType = typeof(bool);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, attemptedContainerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotSupportedException>()
			.Which.ContainerType.Should().Be(attemptedContainerType);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromElement_ElementPropertyMapping_ReturnsValue()
	{
		// Arrange
		var expected = new TestClass
		{
			Value = "test"
		};
		var input = Element(nameof(TestClass),
			Element(nameof(TestClass.Value), Text("test"))
		);

		var type = typeof(TestClass);
		var containerType = typeof(IXmlElement);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.DeserializeFromElement(input, type, _coreContextStub);
		var typedResult = sut.DeserializeFromElement<TestClass>(input, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
		typedResult.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromAttribute_AttributePropertyMapping_ReturnsValue()
	{
		// Arrange
		var expected = new TestClass
		{
			Value = "test"
		};
		var input = Element(nameof(TestClass),
			Attribute(nameof(TestClass.Value), "test")
		);

		var type = typeof(TestClass);
		var containerType = typeof(IXmlAttribute);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromText_TextPropertyMapping_ReturnsValue()
	{
		// Arrange
		var expected = new TestClass
		{
			Value = "test"
		};
		var input = Element(nameof(TestClass),
			Text("test")
		);

		var type = typeof(TestClass);
		var containerType = typeof(IXmlText);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	private sealed class TestClass
	{
		public string Value { get; init; } = default!;
	}
}
