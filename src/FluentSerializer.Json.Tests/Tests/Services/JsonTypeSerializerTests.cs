using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Profiles;
using FluentSerializer.Json.Services;

using Moq;

using System;
using System.Collections.Generic;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.Services;

public sealed class JsonTypeSerializerTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Serialize;

	private readonly ISerializerCoreContext<IJsonNode> _coreContextStub;
	private readonly Mock<IClassMap> _classMapMock;
	private readonly Mock<IClassMapCollection> _classMapCollectionMock;

	public JsonTypeSerializerTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_coreContextStub = new SerializerCoreContext<IJsonNode>(serializerMock.Object);
		_classMapMock = new Mock<IClassMap>()
			.WithDefaults()
			.WithoutPropertyMaps();
		_classMapCollectionMock = new Mock<IClassMapCollection>();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Initialize_NullValue_Throws()
	{
		// Act

		var result = () => new JsonTypeSerializer(null!);
		// Assert
		result.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("classMapCollection");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void SerializeToNode_NullValues_Throws()
	{
		// Arrange
		var dataModel = new TestClass();
		var classType = typeof(TestClass);

		var sut = new JsonTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result1 = () => sut.SerializeToNode(null!, classType, _coreContextStub);
		var result2 = () => sut.SerializeToNode(dataModel, null!, _coreContextStub);
		var result3 = () => sut.SerializeToNode(dataModel, classType, null!);

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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void SerializeToNode_TypeIsNotRegistered_Throws()
	{
		// Arrange
		var input = new TestClass();

		var type = typeof(TestClass);

		_classMapCollectionMock
			.WithoutClassMaps();

		var sut = new JsonTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.SerializeToNode(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ClassMapNotFoundException>()
			.Which.TargetType.Should().Be(type);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void SerializeToNode_NoPropertiesMapped_ReturnsEmptyNode()
	{
		// Arrange
		var expected = Object();
		var input = new TestClass
		{
			Value = "Never used"
		};

		var type = typeof(TestClass);

		_classMapMock
			.WithClassType(type);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new JsonTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.SerializeToNode(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void SerializeToNode_InvalidPropertyMapping_Throws()
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
			.WithBasicPropertyMapping(TestDirection, attemptedContainerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new JsonTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.SerializeToNode(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotSupportedException>()
			.Which.ContainerType.Should().Be(attemptedContainerType);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void SerializeToNode_ObjectPropertyMapping_ReturnsValue()
	{
		// Arrange
		var expected = Object(
			Property(nameof(TestClass.Value), Value("\"test\""))
		);
		var input = new TestClass
		{
			Value = "test"
		};

		var type = typeof(TestClass);
		var containerType = typeof(IJsonProperty);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicPropertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new JsonTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.SerializeToNode(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void SerializeToNode_TypeIsEnumerable_ReturnsArray()
	{
		// Arrange
		var expected = Array();
		var input = new List<int>();

		var type = typeof(IEnumerable<int>);

		_classMapCollectionMock
			.WithoutClassMaps();

		var sut = new JsonTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.SerializeToNode(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		InlineData(ReferenceLoopBehavior.Throw), InlineData(ReferenceLoopBehavior.Ignore)]
	public void SerializeToNode_NoReferenceLoop_ReturnsObject(ReferenceLoopBehavior behavior)
	{
		// Arrange
		var expected = Object(
			Property(nameof(TestClass.Value), Value("\"test\""))
		);
		var input = new TestClass
		{
			Value = "test"
		};

		var type = typeof(TestClass);
		var containerType = typeof(IJsonProperty);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicPropertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var configuration = new JsonSerializerConfiguration
		{
			ReferenceLoopBehavior = behavior,
			DefaultNamingStrategy = Names.Use.PascalCase
		};
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(configuration);
		var contextStub = new SerializerCoreContext<IJsonNode>(serializerMock.Object);

		var sut = new JsonTypeSerializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.SerializeToNode(input, type, contextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void SerializeToNode_ReferenceLoop_BehaviorIgnore_ReturnsObject()
	{
		// Arrange
		var expected1 = Object(
			Property(nameof(TestClass.Value), Value("\"test\"")),
			Property(nameof(TestClass.WrappedReference), Value("null"))
		);
		var input1 = new TestClass
		{
			Value = "test"
		};
		input1.Reference = input1;

		var expected2 = Object(
			Property(nameof(TestClass.Value), Value("\"test\"")),
			Property(nameof(TestClass.Reference), Value("null")),
			Property(nameof(TestClass.WrappedReference), Object(
				Property(nameof(TestClass.Value), Value("\"test\""))
			))
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
		var configuration = new JsonSerializerConfiguration
		{
			ReferenceLoopBehavior = ReferenceLoopBehavior.Ignore,
			DefaultNamingStrategy = Names.Use.PascalCase,
			WriteNull = true
		};
		var testClassMaps = ((ISerializerProfile<JsonSerializerConfiguration>)new TestClassProfile()).Configure(configuration);

		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(configuration);
		var contextStub = new SerializerCoreContext<IJsonNode>(serializerMock.Object);

		var sut = new JsonTypeSerializer(new ClassMapCollection(testClassMaps));

		// Act
		var result1 = sut.SerializeToNode(input1, type, contextStub);
		var result2 = sut.SerializeToNode(input2, type, contextStub);

		// Assert
		result1.Should().BeEquivalentTo(expected1);
		result2.Should().BeEquivalentTo(expected2);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void SerializeToNode_ReferenceLoop_BehaviorThrow_Throws()
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
		var configuration = new JsonSerializerConfiguration
		{
			ReferenceLoopBehavior = ReferenceLoopBehavior.Throw,
			DefaultNamingStrategy = Names.Use.PascalCase
		};
		var testProfile = ((ISerializerProfile<JsonSerializerConfiguration>)new TestClassProfile()).Configure(configuration);

		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(configuration);
		var contextStub = new SerializerCoreContext<IJsonNode>(serializerMock.Object);

		var sut = new JsonTypeSerializer(new ClassMapCollection(testProfile));

		// Act
		var result1 = () => sut.SerializeToNode(input1, type, contextStub);
		var result2 = () => sut.SerializeToNode(input2, type, contextStub);

		// Assert
		result1.Should().ThrowExactly<ReferenceLoopException>()
			.Which.InstanceType.Should().Be(typeof(TestClass));
		result2.Should().ThrowExactly<ReferenceLoopException>()
			.Which.InstanceType.Should().Be(typeof(TestWrapper));
	}

	private sealed class TestClassProfile : JsonSerializerProfile
	{
		protected override void Configure()
		{
			For<TestClass>()
				.Property(test => test.Value)
				.Property(test => test.Reference)
				.Property(test => test.WrappedReference);

			For<TestWrapper>()
				.Property(test => test.Value)
				.Property(test => test.Reference);
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
