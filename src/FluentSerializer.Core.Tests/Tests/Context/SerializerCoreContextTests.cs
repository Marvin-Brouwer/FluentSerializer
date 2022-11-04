using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using FluentSerializer.Core.Tests.ObjectMother;

using Moq;

using System;
using System.Reflection;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Context;

public sealed class SerializerCoreContextTests
{
	private readonly SerializerCoreContext _sut;

	public SerializerCoreContextTests()
	{
		var serializerMock = new Mock<ISerializer>()
			.UseConfig(TestSerializerConfiguration.Default);

		_sut = new SerializerCoreContext(serializerMock.Object);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void TryAddReference_NullValue_ReturnsFalse()
	{
		// Arrange
		var value = (object?)null;

		// Act
		var result = _sut
			.TryAddReference(value);

		// Assert
		result.Should().BeFalse();
		_sut.ContainsReference(value).Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void TryAddReference_AnyValue_ReturnsTrueAndContainsValue()
	{
		// Arrange
		var value = new TestClass();

		// Act
		var result = _sut
			.TryAddReference(value);

		// Assert
		result.Should().BeTrue();
		_sut.ContainsReference(value).Should().BeTrue();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void TryAddReference_AlreadyHasValue_ReturnsFalse()
	{
		// Arrange
		var value = new TestClass();

		_sut.TryAddReference(value);

		// Act
		var result = _sut
			.TryAddReference(value);

		// Assert
		result.Should().BeFalse();
		_sut.ContainsReference(value).Should().BeTrue();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithPathSegment_Class_ReturnsExpectedPath()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		var result = _sut
			.WithPathSegment(type);

		// Assert
		result.Path.Should().BeEquivalentTo("T:TestClass");
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithPathSegment_Property_ReturnsExpectedPath()
	{
		// Arrange
		var type = typeof(TestClass);
		var property = type.GetProperty(nameof(TestClass.Id))!;

		// Act
		var result = _sut
			.WithPathSegment(property);

		// Assert
		result.Path.Should().BeEquivalentTo("P:Id");
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithPathSegment_ClassAndProperty_ReturnsExpectedPath()
	{
		// Arrange
		var type = typeof(TestClass);
		var property = type.GetProperty(nameof(TestClass.Id))!;

		// Act
		var result = _sut
			.WithPathSegment(type)
			.WithPathSegment(property);

		// Assert
		result.Path.Should().BeEquivalentTo("T:TestClass", "P:Id");
	}


	[Theory,
		InlineData(null, null),
		InlineData(typeof (TestClass), null),
		Trait("Category", "UnitTest")]
	public void WithPathSegment_ValueNull_Throws(Type type, PropertyInfo property)
	{
		// Act
		var result = () => _sut
			.WithPathSegment(type)
			.WithPathSegment(property);

		// Assert
		result.Should().ThrowExactly<ArgumentNullException>();
	}

	private sealed class TestClass
	{
		public int Id { get; init; } = default!;
	}

	private sealed class TestSerializerConfiguration : SerializerConfiguration
	{
		public static readonly TestSerializerConfiguration Default = new ();
	}
}