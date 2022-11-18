using FluentAssertions;

using FluentSerializer.Core.Converting.Converters;

using System;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.ToString(bool)"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </summary>
public sealed class SimpleTypeConverterBaseTests
{
	private readonly TestConverter _sut;

	public SimpleTypeConverterBaseTests()
	{
		_sut = new TestConverter();
	}

	#region ConvertToNullableDataType

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_Convertible_ReturnsValue()
	{
		// Arrange
		var input = "TestValue";
		var expected = new TestClass(input);

		// Act
		var canConvert = _sut.CanConvert(expected.GetType());
		var result = _sut.ConvertToNullableDataType(input);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_Convertible_NullValue_ReturnsNull()
	{
		// Act
		var result = _sut.ConvertToNullableDataType(null);

		// Assert
		result.Should().BeNull();
	}

	#endregion

	/// <inheritdoc cref="SimpleTypeConverterBase{T}"/>
	private sealed class TestConverter : SimpleTypeConverterBase<TestClass>
	{
		/// <inheritdoc />
		protected override string ConvertToString(in TestClass value) => value.ToString();

		/// <inheritdoc />
		protected override TestClass ConvertToDataType(in string currentValue) => new(currentValue);

		/// <inheritdoc cref="ConvertibleConverterBase.ConvertToNullableDataType"/>
		public new TestClass? ConvertToNullableDataType(in string? currentValue) =>
			base.ConvertToNullableDataType(in currentValue);
	}

	private sealed record TestClass(string Input);

}

