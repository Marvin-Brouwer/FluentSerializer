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
using FluentSerializer.Json.Tests.ObjectMother;
using Moq;
using System.Collections.Generic;
using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.Services;

public sealed class JsonTypeSerializerTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Serialize;

	private readonly ISerializerCoreContext<IJsonNode> _coreContextStub;
	private readonly Mock<IClassMapScanList<JsonSerializerProfile>> _scanList;
	private readonly Mock<IClassMap> _classMap;

	public JsonTypeSerializerTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_coreContextStub = new SerializerCoreContext<IJsonNode>(serializerMock.Object);
		_scanList = new Mock<IClassMapScanList<JsonSerializerProfile>>();
		_classMap = new Mock<IClassMap>()
			.WithNamingStrategy(Names.Use.PascalCase)
			.WithoutPropertyMaps();
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
		var scanList = Mock.Of<IClassMapScanList<JsonSerializerProfile>>();

		var sut = new JsonTypeSerializer(in scanList);

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
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeSerializer(_scanList.Object);

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
		_classMap
			.WithBasicProppertyMapping(TestDirection, attemptedContainerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeSerializer(_scanList.Object);

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
		_classMap
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeSerializer(_scanList.Object);

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
		var scanList = Mock.Of<IClassMapScanList<JsonSerializerProfile>>();

		var sut = new JsonTypeSerializer(in scanList);

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
		_classMap
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var configuration = new JsonSerializerConfiguration
		{
			ReferenceLoopBehavior = behavior,
			DefaultNamingStrategy = Names.Use.PascalCase
		};
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(configuration);
		var contextStub = new SerializerCoreContext<IJsonNode>(serializerMock.Object);

		var sut = new JsonTypeSerializer(_scanList.Object);

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
		var testProfile = ((ISerializerProfile)new TestClassProfile()).Configure(configuration);
		var scanList = new ClassMapScanList<JsonSerializerProfile>(testProfile);

		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(configuration);
		var contextStub = new SerializerCoreContext<IJsonNode>(serializerMock.Object);

		var sut = new JsonTypeSerializer(scanList);

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
		var testProfile = ((ISerializerProfile)new TestClassProfile()).Configure(configuration);
		var scanList = new ClassMapScanList<JsonSerializerProfile>(testProfile);

		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(configuration);
		var contextStub = new SerializerCoreContext<IJsonNode>(serializerMock.Object);

		var sut = new JsonTypeSerializer(scanList);

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
