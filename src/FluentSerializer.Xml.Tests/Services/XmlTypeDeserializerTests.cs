using FluentAssertions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.TestUtils.ObjectMother;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Exceptions;
using FluentSerializer.Xml.Profiles;
using FluentSerializer.Xml.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Services;

public sealed class XmlTypeDeserializerTests
{
	//private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;
	private readonly Mock<IAdvancedXmlSerializer> _serializerMock;

	public XmlTypeDeserializerTests()
	{
		_serializerMock = new Mock<IAdvancedXmlSerializer>();
		//_contextMock = new Mock<ISerializerContext<IXmlNode>>()
		//	.SetupDefault(_serializerMock)
		//	.WithNamingStrategy(Names.Use.CamelCase);
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
		var type = typeof(IEnumerable<int>);
		var scanList = Mock.Of<IClassMapScanList<XmlSerializerProfile>>();

		var sut  = new XmlTypeDeserializer(in scanList);

		// Act
		var result = () => sut.DeserializeFromElement(
			Element("PlaceHolder"), type, _serializerMock.Object);

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
		var type = typeof(TestClass);
		var scanList = Mock.Of<IClassMapScanList<XmlSerializerProfile>>();

		var sut = new XmlTypeDeserializer(in scanList);

		// Act
		var result = () => sut.DeserializeFromElement(
			Element("PlaceHolder"), type, _serializerMock.Object);

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
		var classMap = new Mock<IClassMap>();
		classMap
			.Setup(cMap => cMap.NamingStrategy)
			.Returns(Names.Use.PascalCase);
		var scanList = new Mock<IClassMapScanList<XmlSerializerProfile>>();
		scanList
			.Setup(list => list.Scan(It.Is((
				(Type type, SerializerDirection direction) scanFor) => scanFor.type == type))
			)
			.Returns(classMap.Object);

		var sut = new XmlTypeDeserializer(scanList.Object);

		// Act
		var result = () => sut.DeserializeFromElement(
			Element("IncorrectName"), type, _serializerMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<MissingNodeException>()
			.Which.AttemptedType.Should().Be(type);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void DeserializeFromElement_NoPropertiesMapped_ReturnsEmptyNode()
	{
		// Arrange
		var input = Element(nameof(TestClass));
		var type = typeof(TestClass);

		var propertyMap = new Mock<IScanList<PropertyInfo, IPropertyMap>>();
		propertyMap
			.Setup(pMap => pMap.GetEnumerator())
			.Returns(Enumerable.Empty<IPropertyMap>().GetEnumerator());
		var classMap = new Mock<IClassMap>();
		classMap
			.Setup(cMap => cMap.NamingStrategy)
			.Returns(Names.Use.PascalCase);
		classMap
			.Setup(cMap => cMap.PropertyMaps)
			.Returns(propertyMap.Object);
		var scanList = new Mock<IClassMapScanList<XmlSerializerProfile>>();
		scanList
			.Setup(list => list.Scan(It.Is((
				(Type type, SerializerDirection direction) scanFor) => scanFor.type == type))
			)
			.Returns(classMap.Object);

		var sut = new XmlTypeDeserializer(scanList.Object);

		// Act
		var result = sut.DeserializeFromElement(input, type, _serializerMock.Object);

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
		// Any aribitrary type here
		var attemptedContainerType = typeof(bool);
		var propertyMapping = new PropertyMap(SerializerDirection.Deserialize,
			attemptedContainerType,
			type.GetProperty(nameof(TestClass.Name))!, Names.Use.PascalCase, null);

		var propertyMap = new Mock<IScanList<PropertyInfo, IPropertyMap>>();
		propertyMap
			.Setup(pMap => pMap.GetEnumerator())
			.Returns(new List<IPropertyMap> { propertyMapping }.GetEnumerator());
		var classMap = new Mock<IClassMap>();
		classMap
			.Setup(cMap => cMap.NamingStrategy)
			.Returns(Names.Use.PascalCase);
		classMap
			.Setup(cMap => cMap.PropertyMaps)
			.Returns(propertyMap.Object);
		var scanList = new Mock<IClassMapScanList<XmlSerializerProfile>>();
		scanList
			.Setup(list => list.Scan(It.Is((
				(Type type, SerializerDirection direction) scanFor) => scanFor.type == type))
			)
			.Returns(classMap.Object);

		var sut = new XmlTypeDeserializer(scanList.Object);

		// Act
		var result = () => sut.DeserializeFromElement(input, type, _serializerMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotSupportedException>()
			.Which.ContainerType.Should().Be(attemptedContainerType);
	}

	// Todo 1 element

	// Todo 1 attribute

	// Todo 1 text

	// Todo 1 comment
	private sealed class TestClass
	{
		public string Name { get; set; } = default!;
	}
}
