using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Exceptions;
using FluentSerializer.Xml.Profiles;
using FluentSerializer.Xml.Services;
using FluentSerializer.Xml.Tests.ObjectMother;

using Moq;

using System;
using System.Collections.Generic;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Services;

public sealed class XmlTypeSerializerTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Serialize;

	private readonly Mock<IAdvancedXmlSerializer> _serializerMock;
	private readonly ISerializerCoreContext<IXmlNode> _coreContextStub;
	private readonly Mock<IClassMap> _classMapMock;
	private readonly Mock<IClassMapCollection> _classMapCollectionMock;

	public XmlTypeSerializerTests()
	{
		_serializerMock = new Mock<IAdvancedXmlSerializer>()
			.UseConfig(XmlSerializerConfiguration.Default);
		_coreContextStub = new SerializerCoreContext<IXmlNode>(_serializerMock.Object);
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

		var result = () => new XmlTypeSerializer(null!);
		// Assert
		result.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("classMapCollection");
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

		_classMapCollectionMock
			.WithoutClassMaps();

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<MalConfiguredRootNodeException>()
			.Which.AttemptedType.Should().Be(type);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToElement_NullValues_Throws()
	{
		// Arrange
		var dataModel = new TestClass();
		var classType = typeof(TestClass);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result1 = () => sut.SerializeToElement(null!, classType, _coreContextStub);
		var result2 = () => sut.SerializeToElement(dataModel, null!, _coreContextStub);
		var result3 = () => sut.SerializeToElement(dataModel, classType, null!);

		// Assert
		result1.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(dataModel));
		result2.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(classType));
		result3.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("coreContext");
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
		_classMapCollectionMock
			.WithoutClassMaps();

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

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

		_classMapMock
			.WithClassType(type);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

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

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, attemptedContainerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

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

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToElement_ElementPropertyMapping_NullValue_ReturnsValue()
	{
		// Arrange
		var expectedNoNull = Element(nameof(TestClass));
		var expectedNull = Element(nameof(TestClass),
			Element(nameof(TestClass.WrappedReference))
		);
		var input = new TestClass
		{
			WrappedReference = null
		};

		var type = typeof(TestClass);
		var containerType = typeof(IXmlElement);
		var targetProperty = type.GetProperty(nameof(TestClass.WrappedReference))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		_serializerMock
			.UseConfig(new XmlSerializerConfiguration
			{
				WriteNull = false
			});
		var resultNoNull = sut.SerializeToElement(input, type, _coreContextStub);

		_serializerMock
			.UseConfig(new XmlSerializerConfiguration
			{
				WriteNull = true
			});
		var resultNull = sut.SerializeToElement(input, type, _coreContextStub);


		// Assert
		resultNoNull.Should().BeEquivalentTo(expectedNoNull);
		resultNull.Should().BeEquivalentTo(expectedNull);
	}

	/// <summary>
	/// Is this correct? <br />
	/// Do we want to throw on illegal null values on serializing? <br />
	/// Maybe add a setting for this? <br />
	/// <see href="https://github.com/Marvin-Brouwer/FluentSerializer/discussions/288"/>
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToElement_ElementPropertyMapping_IllegalNullValue_DoesntThrow()
	{
		// Arrange
		var input = new TestClass
		{
			Value = null!
		};

		var type = typeof(TestClass);
		var containerType = typeof(IXmlElement);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should().NotThrow();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToAttribute_AttributePropertyMapping_ReturnsValue()
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

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToAttribute_AttributePropertyMapping_NullValue_ReturnsValue()
	{
		// Arrange
		var expectedNoNull = Element(nameof(TestClass));
		var expectedNull = Element(nameof(TestClass),
			Attribute(nameof(TestClass.WrappedReference), null)
		);
		var input = new TestClass
		{
			WrappedReference = null
		};

		var type = typeof(TestClass);
		var containerType = typeof(IXmlAttribute);
		var targetProperty = type.GetProperty(nameof(TestClass.WrappedReference))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		_serializerMock
			.UseConfig(new XmlSerializerConfiguration
			{
				WriteNull = false
			});
		var resultNoNull = sut.SerializeToElement(input, type, _coreContextStub);

		_serializerMock
			.UseConfig(new XmlSerializerConfiguration
			{
				WriteNull = true
			});
		var resultNull = sut.SerializeToElement(input, type, _coreContextStub);


		// Assert
		resultNoNull.Should().BeEquivalentTo(expectedNoNull);
		resultNull.Should().BeEquivalentTo(expectedNull);
	}

	/// <summary>
	/// Is this correct? <br />
	/// Do we want to throw on illegal null values on serializing? <br />
	/// Maybe add a setting for this? <br />
	/// <see href="https://github.com/Marvin-Brouwer/FluentSerializer/discussions/288"/>
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToAttribute_AttributePropertyMapping_IllegalNullValue_DoesntThrow()
	{
		// Arrange
		var input = new TestClass
		{
			Value = null!
		};

		var type = typeof(TestClass);
		var containerType = typeof(IXmlAttribute);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should().NotThrow();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToText_TextPropertyMapping_ReturnsValue()
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

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToText_TextPropertyMapping_NullValue_ReturnsValue()
	{
		// Arrange
		var expectedNoNull = Element(nameof(TestClass));
		var expectedNull = Element(nameof(TestClass),
			Text(null)
		);
		var input = new TestClass
		{
			WrappedReference = null
		};

		var type = typeof(TestClass);
		var containerType = typeof(IXmlText);
		var targetProperty = type.GetProperty(nameof(TestClass.WrappedReference))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		_serializerMock
			.UseConfig(new XmlSerializerConfiguration
			{
				WriteNull = false
			});
		var resultNoNull = sut.SerializeToElement(input, type, _coreContextStub);

		_serializerMock
			.UseConfig(new XmlSerializerConfiguration
			{
				WriteNull = true
			});
		var resultNull = sut.SerializeToElement(input, type, _coreContextStub);


		// Assert
		resultNoNull.Should().BeEquivalentTo(expectedNoNull);
		resultNull.Should().BeEquivalentTo(expectedNull);
	}

	/// <summary>
	/// Is this correct? <br />
	/// Do we want to throw on illegal null values on serializing? <br />
	/// Maybe add a setting for this? <br />
	/// <see href="https://github.com/Marvin-Brouwer/FluentSerializer/discussions/288"/>
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToText_TextPropertyMapping_IllegalNullValue_DoesntThrow()
	{
		// Arrange
		var input = new TestClass
		{
			Value = null!
		};

		var type = typeof(TestClass);
		var containerType = typeof(IXmlText);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.SerializeToElement(input, type, _coreContextStub);

		// Assert
		result.Should().NotThrow();
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		InlineData(ReferenceLoopBehavior.Throw), InlineData(ReferenceLoopBehavior.Ignore)]
	public void SerializeToElement_NoReferenceLoop_ReturnsElement(ReferenceLoopBehavior behavior)
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

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var configuration = new XmlSerializerConfiguration
		{
			ReferenceLoopBehavior = behavior,
			DefaultClassNamingStrategy = Names.Use.PascalCase,
			DefaultPropertyNamingStrategy = Names.Use.PascalCase
		};
		var serializerMock = new Mock<IAdvancedXmlSerializer>()
			.UseConfig(configuration);
		var contextStub = new SerializerCoreContext<IXmlNode>(serializerMock.Object);

		var sut = new XmlTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.SerializeToElement(input, type, contextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToElement_ReferenceLoop_BehaviorIgnore_ReturnsElement()
	{
		// Arrange
		var expected1 = Element(nameof(TestClass),
			Attribute(nameof(TestClass.Value), "test"),
			Element(nameof(TestClass.WrappedReference))
		);
		var input1 = new TestClass
		{
			Value = "test"
		};
		input1.Reference = input1;

		var expected2 = Element(nameof(TestClass),
			Attribute(nameof(TestClass.Value), "test"),
			Element(nameof(TestClass.Reference)),
			Element(nameof(TestClass.WrappedReference),
				Element(nameof(TestWrapper),
					Attribute(nameof(TestWrapper.Value), "testWrapper")
				)
			)
		);
		var input2 = new TestClass
		{
			Value = "test"
		};
		input2.WrappedReference = new TestWrapper
		{
			Value = "testWrapper",
			Reference = input2
		};

		var type = typeof(TestClass);
		var configuration = new XmlSerializerConfiguration
		{
			ReferenceLoopBehavior = ReferenceLoopBehavior.Ignore,
			DefaultClassNamingStrategy = Names.Use.PascalCase,
			DefaultPropertyNamingStrategy = Names.Use.PascalCase,
			WriteNull = true
		};
		var testClassMaps = ((ISerializerProfile<XmlSerializerConfiguration>)new TestClassProfile()).Configure(configuration);

		var serializerMock = new Mock<IAdvancedXmlSerializer>()
			.UseConfig(configuration);
		var contextStub = new SerializerCoreContext<IXmlNode>(serializerMock.Object);

		var sut = new XmlTypeSerializer(new ClassMapCollection(testClassMaps));

		// Act
		var result1 = sut.SerializeToElement(input1, type, contextStub);
		var result2 = sut.SerializeToElement(input2, type, contextStub);

		// Assert
		result1.Should().BeEquivalentTo(expected1);
		result2.Should().BeEquivalentTo(expected2);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializeToElement_ReferenceLoop_BehaviorThrow_Throws()
	{
		// Arrange
		var input1 = new TestClass
		{
			Value = "test"
		};
		input1.Reference = input1;

		var input2 = new TestClass
		{
			Value = "test"
		};
		input2.WrappedReference = new TestWrapper
		{
			Value = "testWrapper",
			Reference = input2
		};

		var type = typeof(TestClass);
		var configuration = new XmlSerializerConfiguration
		{
			ReferenceLoopBehavior = ReferenceLoopBehavior.Throw,
			DefaultClassNamingStrategy = Names.Use.PascalCase,
			DefaultPropertyNamingStrategy = Names.Use.PascalCase
		};
		var testClassMaps = ((ISerializerProfile<XmlSerializerConfiguration>)new TestClassProfile()).Configure(configuration);

		var serializerMock = new Mock<IAdvancedXmlSerializer>()
			.UseConfig(configuration);
		var contextStub = new SerializerCoreContext<IXmlNode>(serializerMock.Object);

		var sut = new XmlTypeSerializer(new ClassMapCollection(testClassMaps));

		// Act
		var result1 = () => sut.SerializeToElement(input1, type, contextStub);
		var result2 = () => sut.SerializeToElement(input2, type, contextStub);

		// Assert
		result1.Should().ThrowExactly<ReferenceLoopException>()
			.Which.InstanceType.Should().Be(typeof(TestClass));
		result2.Should().ThrowExactly<ReferenceLoopException>()
			.Which.InstanceType.Should().Be(typeof(TestWrapper));
	}

	private sealed class TestClassProfile : XmlSerializerProfile
	{
		protected override void Configure()
		{
			For<TestClass>()
				.Attribute(test => test.Value)
				.Child(test => test.Reference)
				.Child(test => test.WrappedReference);

			For<TestWrapper>()
				.Attribute(test => test.Value)
				.Child(test => test.Reference);
		}
	}

	private sealed class TestClass
	{
		public string Value { get; init; } = default!;
		public TestClass? Reference { get; set; }
		public TestWrapper? WrappedReference { get; set; }
	}

	private sealed class TestWrapper
	{
		public string Value { get; init; } = default!;
		public TestClass? Reference { get; init; }
	}
}
