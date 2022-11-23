using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;

using Moq;

using System;

using Xunit;


namespace FluentSerializer.Core.Tests.Tests.Converting;

public sealed class ConverterComparerTests
{
	private static readonly ConverterComparer Sut = ConverterComparer.Default;

	[Fact]
	public void Equals_BothAreNull_ReturnsTrue()
	{
		// Arrange
		IConverter? a = null;
		IConverter? b = null;

		// Act
		var result = Sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Equals_OneIsNull_ReturnsFalse()
	{
		// Arrange
		IConverter? a = null;
		IConverter b = Mock.Of<IConverter>();
		IConverter? c = null;

		// Act
		var resultLeft = Sut.Equals(a, b);
		var resultRight = Sut.Equals(b, c);

		// Assert
		resultLeft.Should().BeFalse();
		resultRight.Should().BeFalse();
	}

	[Fact]
	public void Equals_ReferenceEqual_ReturnsTrue()
	{
		// Arrange
		var a = new TestConverter();
		var b = a;

		// Act
		var result = Sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Equals_AreEqual_ReturnsTrue()
	{
		// Arrange
		var a = new TestConverter();
		var b = new TestConverter();

		// Act
		var result = Sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Equals_AreNotEqual_ReturnsFalse()
	{
		// Arrange
		var guidA = new Guid("5266C721-40AD-4C31-8662-BFB592DD7D2B");
		var guidB = new Guid("676E602E-8463-471E-9D11-DA57FC8F8822");

		var a = new TestConverter(guidA);
		var b = new TestConverter(guidB);
		var c = new TestConverter(guidA);

		// Act
		var resultName = Sut.Equals(a, b);
		var resultValue = Sut.Equals(b, c);

		// Assert
		resultName.Should().BeFalse();
		resultValue.Should().BeFalse();
	}

	private sealed class TestConverter : IConverter
	{
		public TestConverter() : this(typeof(TestConverter).GUID) { }
		public TestConverter(Guid guid)
		{
			ConverterId = guid;
		}

		public Guid ConverterId { get; }

		public SerializerDirection Direction => SerializerDirection.Both;
		public bool CanConvert(in Type targetType) => true;
	}
}