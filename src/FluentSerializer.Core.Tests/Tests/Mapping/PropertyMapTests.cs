using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Services;
using FluentSerializer.Core.Text;

using Moq;

using System;
using System.Linq;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Mapping;

public sealed class PropertyMapTests
{
	private readonly Mock<ISerializer> _serializerMock;
	private readonly Mock<IConfigurationStack<IConverter>> _configStackMock;

	private static PropertyMap CreateSut(IConverter? converter)
	{
		const SerializerDirection direction = SerializerDirection.Serialize;
		var type = typeof(TestClass);
		var nullableProperty = typeof(TestClass).GetProperty(nameof(TestClass.NullableProperty))!;
		var namingStrategy = Names.Use.CamelCase;

		return new PropertyMap(direction, type, nullableProperty, namingStrategy,
			converter is null ? null : () => converter);
	}

	public PropertyMapTests()
	{
		_configStackMock = new Mock<IConfigurationStack<IConverter>>(MockBehavior.Loose);
		_configStackMock
			.Setup(configStack => configStack.GetEnumerator())
			.Returns(Enumerable.Empty<IConverter>().GetEnumerator());

		_serializerMock = new Mock<ISerializer>(MockBehavior.Loose);
		_serializerMock
			.SetupGet(serializer => serializer.Configuration)
			.Returns(new TestConfiguration
			{
				DefaultConverters = _configStackMock.Object
			});
	}

	#region NewPropertyMap

	[Fact,
		Trait("Category", "UnitTest")]
	public void NewPropertyMap_NullValues_Throws()
	{
		// Arrange
		var containerType = typeof(TestClass);
		var property = typeof(TestClass).GetProperty(nameof(TestClass.NullableProperty))!;
		var namingStrategy = Names.Use.CamelCase;

		// Act
		var result1 = () => new PropertyMap(SerializerDirection.Serialize, null!, property, namingStrategy!, null);
		var result2 = () => new PropertyMap(SerializerDirection.Serialize, containerType, null!, namingStrategy!, null);
		var result3 = () => new PropertyMap(SerializerDirection.Serialize, containerType!, property, null!, null);
		var result4 = () => new PropertyMap(SerializerDirection.Serialize, containerType, property, namingStrategy, null);

		// Assert
		result1.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(containerType));
		result2.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(property));
		result3.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(namingStrategy));
		result4.Should()
			.NotThrow("This argument is optional");
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void NewPropertyMap_AnyType_HasConcreteType()
	{
		// Arrange
		var nullableProperty = typeof(TestClass).GetProperty(nameof(TestClass.NullableProperty))!;
		var concreteProperty = typeof(TestClass).GetProperty(nameof(TestClass.ConcreteProperty))!;

		// Act
		var nullableResult = new PropertyMap(SerializerDirection.Serialize, typeof(int), nullableProperty, Names.Use.CamelCase, null);
		var concreteResult = new PropertyMap(SerializerDirection.Serialize, typeof(int), concreteProperty, Names.Use.CamelCase, null);

		// Assert
		nullableResult.ConcretePropertyType.Should().Be(concreteProperty.PropertyType);
		nullableResult.ConcretePropertyType.Should().NotBe(nullableProperty.PropertyType);
		concreteResult.ConcretePropertyType.Should().Be(concreteProperty.PropertyType);
		concreteResult.ConcretePropertyType.Should().NotBe(nullableProperty.PropertyType);
	}

	#endregion
	
	#region GetConverter

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetConverter_NoneAvailable_ReturnsNull()
	{
		// Arrange
		const SerializerDirection direction = SerializerDirection.Serialize;

		var sut = CreateSut(null);

		// Act
		var result = sut.GetConverter<TestClass, TestClass>(direction, _serializerMock.Object);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetConverter_ConverterCanNotConvert_Throws()
	{
		// Arrange
		const SerializerDirection direction = SerializerDirection.Serialize;

		var sut = CreateSut(new TestConverter<TestClass>(false));

		// Act
		var result = () => sut.GetConverter<TestClass, TestClass>(direction, _serializerMock.Object);

		// Assert
		result.Should().ThrowExactly<ConverterNotSupportedException>();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetConverter_ConverterIncorrectlyTyped_Throws()
	{
		// Arrange
		const SerializerDirection direction = SerializerDirection.Serialize;

		var sut = CreateSut(new TestConverter<TestClass2>(true));

		// Act
		var result = () => sut.GetConverter<TestClass, TestClass>(direction, _serializerMock.Object);

		// Assert
		result.Should().ThrowExactly<ConverterNotSupportedException>();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetConverter_ConverterCorrect_ReturnsConverter()
	{
		// Arrange
		const SerializerDirection direction = SerializerDirection.Serialize;

		var sut = CreateSut(new TestConverter<TestClass>(true));

		// Act
		var result = sut.GetConverter<TestClass, TestClass>(direction, _serializerMock.Object);

		// Assert
		result.Should().NotBeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetConverter_DefaultConverterFound_ReturnsConverter()
	{
		// Arrange
		const SerializerDirection direction = SerializerDirection.Deserialize;
		var configuredConverters = new IConverter[]
		{
			new TestConverter<TestClass>(true)
		};

		_configStackMock
			.Setup(configurationStack => configurationStack.GetEnumerator())
			.Returns(configuredConverters.AsEnumerable().GetEnumerator());

		var sut = CreateSut(null);

		// Act
		var result = sut.GetConverter<TestClass, TestClass>(direction, _serializerMock.Object);

		// Assert
		result.Should().NotBeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetConverter_SerializerNull_Throws()
	{
		// Arrange
		const SerializerDirection direction = SerializerDirection.Serialize;
		var currentSerializer = (ISerializer)null!;

		var sut = CreateSut(null);

		// Act
		var result = () => sut.GetConverter<TestClass, TestClass>(direction, currentSerializer);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(currentSerializer));
	}

	#endregion

	private sealed record TestClass : IDataNode
	{
		public static int ConcreteProperty => 0;
		public static int? NullableProperty => 0;

		public string Name => nameof(TestClass);

		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true) =>
			stringBuilder
				.Append(ToString())
				.AppendLineEnding();

		public bool Equals(IDataNode? other) => ReferenceEquals(this, other);
	}
	private sealed record TestClass2 : IDataNode
	{
		public string Name => nameof(TestClass2);

		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true) =>
			stringBuilder
				.Append(ToString())
				.AppendLineEnding();

		public bool Equals(IDataNode? other) => ReferenceEquals(this, other);
	}

	private sealed class TestConfiguration : SerializerConfiguration { }

	private sealed class TestConverter<TNode> : IConverter<TNode, TNode>
		where TNode : IDataNode
	{
		private readonly bool _canConvert;

		public TestConverter(bool canConvert)
		{
			_canConvert = canConvert;
		}

		public bool CanConvert(in Type targetType) => _canConvert;

		public SerializerDirection Direction => SerializerDirection.Both;

		public int ConverterHashCode => GetHashCode();

		public TNode? Serialize(in object objectToSerialize, in ISerializerContext context) =>
			throw new NotSupportedException("Out of test scope");

		public object? Deserialize(in TNode objectToDeserialize, in ISerializerContext<TNode> context) =>
			throw new NotSupportedException("Out of test scope");
	}

}