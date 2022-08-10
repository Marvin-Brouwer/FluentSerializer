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

using System.Collections.Generic;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Services;

public sealed class XmlTypeDeserializerTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Deserialize;

	private readonly ISerializerCoreContext<IXmlNode> _coreContextStub;
	private readonly Mock<IClassMap> _classMap;

	public XmlTypeDeserializerTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>()
			.UseConfig(XmlSerializerConfiguration.Default);
		_coreContextStub = new SerializerCoreContext<IXmlNode>(serializerMock.Object);
		_classMap = new Mock<IClassMap>()
			.WithDefaults()
			.WithoutPropertyMaps();
	}

	/// <summary>
	/// We're either parsing a root node or a document.
	/// XML specs that a file cannot have more than one root node.
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void DeserializeFromElement_TypeIsEnumerable_Throws()
	{
		// Arrange
		var input = Element("PlaceHolder");

		var type = typeof(IEnumerable<int>);
		var classMap = new List<IClassMap>(0);

		var sut = new XmlTypeDeserializer(classMap);

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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void DeserializeFromElement_TypeIsNotRegistered_Throws()
	{
		// Arrange
		var input = Element("PlaceHolder");

		var type = typeof(TestClass);
		var classMap = new List<IClassMap>(0);

		var sut = new XmlTypeDeserializer(classMap);

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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void DeserializeFromElement_NodeIsNotPresent_Throws()
	{
		// Arrange
		var type = typeof(TestClass);
		_classMap
			.WithClassType(type);
		
		var sut = new XmlTypeDeserializer(_classMap.ToCollection());

		// Act
		var result = () => sut.DeserializeFromElement(
			Element("IncorrectName"), type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<MissingNodeException>()
			.Which.AttemptedType.Should().Be(type);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void DeserializeFromNode_NonNullableValueIsEmpty_Throws()
	{
		// Arrange
		var input = Element(nameof(TestClass));

		var type = typeof(TestClass);
		var containerType = typeof(IXmlElement);
		var property = type.GetProperty(nameof(TestClass.Value));
		_classMap
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, property!);
		
		var sut = new XmlTypeDeserializer(_classMap.ToCollection());

		// Act
		var result = () => sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotFoundException>()
			.Which.TargetType.Should().Be(containerType);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void DeserializeFromElement_NoPropertiesMapped_ReturnsEmptyNode()
	{
		// Arrange
		var input = Element(nameof(TestClass));

		var type = typeof(TestClass);
		_classMap
			.WithClassType(type);

		var sut = new XmlTypeDeserializer(_classMap.ToCollection());

		// Act
		var result = sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeOfType(type);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void DeserializeFromElement_InvalidPropertyMapping_Throws()
	{
		// Arrange
		var input = Element(nameof(TestClass));
		var type = typeof(TestClass);

		// Any arbitrary type here
		var attemptedContainerType = typeof(bool);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;
		_classMap
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, attemptedContainerType, targetProperty);

		var sut = new XmlTypeDeserializer(_classMap.ToCollection());

		// Act
		var result = () => sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotSupportedException>()
			.Which.ContainerType.Should().Be(attemptedContainerType);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
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
		_classMap
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);

		var sut = new XmlTypeDeserializer(_classMap.ToCollection());

		// Act
		var result = sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
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
		_classMap
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);

		var sut = new XmlTypeDeserializer(_classMap.ToCollection());

		// Act
		var result = sut.DeserializeFromElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
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
		_classMap
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);

		var sut = new XmlTypeDeserializer(_classMap.ToCollection());

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
