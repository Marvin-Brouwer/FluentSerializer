using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Services;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Core.Text;

using Microsoft.Extensions.ObjectPool;

using Moq;

using System;
using System.Reflection;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Context;

/// <remarks>
/// These tests mainly focus on the <see cref="_typedSut"/> since the methods on <see cref="SerializerCoreContext{T}"/> proxy through
/// to the counterparts in <see cref="SerializerCoreContext"/>.
/// However for null checking the regular <see cref="_sut"/> is used, since technically they can be accessed with null values.
/// </remarks>
public sealed class SerializerCoreContextTests
{
	private readonly SerializerCoreContext _sut;
	private readonly SerializerCoreContext<TestClass> _typedSut;

	public SerializerCoreContextTests()
	{
		var serializerMock = new Mock<ISerializer>()
			.UseConfig(TestSerializerConfiguration.Default);

		_sut = new SerializerCoreContext(serializerMock.Object);
		_typedSut = new SerializerCoreContext<TestClass>(serializerMock.Object);
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
		var typedResult = _typedSut
			.TryAddReference(value);

		// Assert
		result.Should().BeFalse();
		_sut.ContainsReference(value).Should().BeFalse();
		typedResult.Should().BeFalse();
		_typedSut.ContainsReference(value).Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void TryAddReference_AnyValue_ReturnsTrueAndContainsValue()
	{
		// Arrange
		var value = new TestClass();

		// Act
		var result = _typedSut
			.TryAddReference(value);

		// Assert
		result.Should().BeTrue();
		_typedSut.ContainsReference(value).Should().BeTrue();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void TryAddReference_AlreadyHasValue_ReturnsFalse()
	{
		// Arrange
		var value = new TestClass();

		_typedSut.TryAddReference(value);

		// Act
		var result = _typedSut
			.TryAddReference(value);

		// Assert
		result.Should().BeFalse();
		_typedSut.ContainsReference(value).Should().BeTrue();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithPathSegment_Class_ReturnsExpectedPath()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		var result = _typedSut
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
		var result = _typedSut
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
		var result = _typedSut
			.WithPathSegment(type)
			.WithPathSegment(property);

		// Assert
		result.Path.Should().BeEquivalentTo("T:TestClass", "P:Id");
	}

	[Theory,
		InlineData(null, null),
		InlineData(typeof(TestClass), null),
		Trait("Category", "UnitTest")]
	public void WithPathSegment_ValueNull_Throws(Type type, PropertyInfo property)
	{
		// Act
		var result = () => _sut
			.WithPathSegment(type)
			.WithPathSegment(property);
		var typedResult = () => _typedSut
			.WithPathSegment(type)
			.WithPathSegment(property);

		// Assert
		result.Should().ThrowExactly<ArgumentNullException>();
		typedResult.Should().ThrowExactly<ArgumentNullException>();
	}

	private sealed class TestClass : IDataNode
	{
		public int Id { get; init; } = default!;

		public bool Equals(IDataNode? other) => false;
		public HashCode GetNodeHash() => DataNodeHashingHelper.GetHashCodeForAll(Id);

		public string Name => throw new NotSupportedException("Out of test scope");
		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true) =>
			throw new NotSupportedException("Out of test scope");
		public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0) =>
			throw new NotSupportedException("Out of test scope");
	}

	private sealed class TestSerializerConfiguration : SerializerConfiguration
	{
		public static readonly TestSerializerConfiguration Default = new();
	}
}