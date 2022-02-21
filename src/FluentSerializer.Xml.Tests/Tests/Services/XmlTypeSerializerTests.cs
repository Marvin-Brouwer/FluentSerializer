using FluentAssertions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Exceptions;
using FluentSerializer.Xml.Profiles;
using FluentSerializer.Xml.Services;
using FluentSerializer.Xml.Tests.ObjectMother;
using Moq;
using System.Collections.Generic;
using FluentSerializer.Core.Context;
using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Services;

public sealed class XmlTypeSerializerTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Serialize;

	private readonly ISerializerCoreContext<IXmlNode> _coreContextStub;
	private readonly Mock<IClassMapScanList<XmlSerializerProfile>> _scanList;
	private readonly Mock<IClassMap> _classMap;

	public XmlTypeSerializerTests()
	{
		_coreContextStub = new SerializerCoreContext<IXmlNode>(Mock.Of<IAdvancedXmlSerializer>());
		_scanList = new Mock<IClassMapScanList<XmlSerializerProfile>>();
		_classMap = new Mock<IClassMap>()
			.WithNamingStrategy(Names.Use.PascalCase)
			.WithoutPropertyMaps();
	}

	/// <summary>
	/// We're either parsing a root node or a document.
	/// XML specs that a file cannot have more than one root node.
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToElement_TypeIsEnumerable_Throws()
	{
		// Arrange
		var input = new List<int>();

		var type = typeof(IEnumerable<int>);
		var scanList = Mock.Of<IClassMapScanList<XmlSerializerProfile>>();

		var sut = new XmlTypeSerializer(in scanList);

		// Act
		var result = () => sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<MalConfiguredRootNodeException>()
			.Which.AttemptedType.Should().Be(type);
	}

	/// <summary>
	/// We need a profile to serialize
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToElement_TypeIsNotRegistered_Throws()
	{
		// Arrange
		var input = new TestClass();

		var type = typeof(TestClass);
		var scanList = Mock.Of<IClassMapScanList<XmlSerializerProfile>>();

		var sut = new XmlTypeSerializer(in scanList);

		// Act
		var result = () => sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ClassMapNotFoundException>()
			.Which.TargetType.Should().Be(type);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToElement_NoPropertiesMapped_ReturnsEmptyNode()
	{
		// Arrange
		var expected = Element(nameof(TestClass));
		var input = new TestClass
		{
			Value = "Never used"
		};

		var type = typeof(TestClass);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new XmlTypeSerializer(_scanList.Object);

		// Act
		var result = sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToElement_InvalidPropertyMapping_Throws()
	{
		// Arrange
		var input = new TestClass
		{
			Value = "Never used"
		};
		var type = typeof(TestClass);

		// Any arbitrary type here
		var attemptedContainerType = typeof(bool);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;
		_classMap
			.WithBasicProppertyMapping(TestDirection, attemptedContainerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new XmlTypeSerializer(_scanList.Object);

		// Act
		var result = () => sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotSupportedException>()
			.Which.ContainerType.Should().Be(attemptedContainerType);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToElement_ElementPropertyMapping_ReturnsValue()
	{
		// Arrange
		var expected = Element(nameof(TestClass),
			Element(nameof(TestClass.Value), Text("test"))
		);
		var input = new TestClass
		{
			Value = "test"
		};

		var type = typeof(TestClass);
		var containerType = typeof(IXmlElement);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;
		_classMap
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new XmlTypeSerializer(_scanList.Object);

		// Act
		var result = sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeFromAttribute_AttributePropertyMapping_ReturnsValue()
	{
		// Arrange
		var expected = Element(nameof(TestClass),
			Attribute(nameof(TestClass.Value), "test")
		);
		var input = new TestClass
		{
			Value = "test"
		};

		var type = typeof(TestClass);
		var containerType = typeof(IXmlAttribute);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;
		_classMap
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new XmlTypeSerializer(_scanList.Object);

		// Act
		var result = sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeFromText_TextPropertyMapping_ReturnsValue()
	{
		// Arrange
		var expected = Element(nameof(TestClass),
			Text("test")
		);
		var input = new TestClass
		{
			Value = "test"
		};

		var type = typeof(TestClass);
		var containerType = typeof(IXmlText);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;
		_classMap
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new XmlTypeSerializer(_scanList.Object);

		// Act
		var result = sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	private sealed class TestClass
	{
		public string Value { get; init; } = default!;
	}
}
