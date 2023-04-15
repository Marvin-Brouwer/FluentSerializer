using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Extensions;

using Moq;

using System;
using System.Globalization;

using Xunit;

namespace FluentSerializer.Json.Tests.Tests.Extensions;

public sealed class UseJsonExtensionsTests
{
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void UseEnum_Default_UseCalled()
	{
		// Arrange
		var configurationStackMock = new Mock<IConfigurationStack<IConverter>>(MockBehavior.Loose);

		// Act
		configurationStackMock.Object.UseEnum(EnumFormats.Default);

		// Assert
		configurationStackMock
			.Verify(
				stack => stack.Use(It.IsAny<Func<IJsonConverter>>(), false),
				Times.Once
			);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void UseParsable_UseCalled()
	{
		// Arrange
		var configurationStackMock = new Mock<IConfigurationStack<IConverter>>(MockBehavior.Loose);

		// Act
		configurationStackMock.Object.UseParsable();
		configurationStackMock.Object.UseParsable(true);
		configurationStackMock.Object.UseParsable(false);
		configurationStackMock.Object.UseParsable(CultureInfo.InvariantCulture);
		configurationStackMock.Object.UseParsable(true, CultureInfo.InvariantCulture);
		configurationStackMock.Object.UseParsable(false, CultureInfo.InvariantCulture);

		// Assert
		configurationStackMock
			.Verify(
				stack => stack.Use(It.IsAny<IJsonConverter>(), It.IsAny<bool>()),
				Times.Once
			);
		configurationStackMock
			.Verify(
				stack => stack.Use(It.IsAny<Func<IJsonConverter>>(), It.IsAny<bool>()),
				Times.Exactly(5)
			);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void UseFormattable_UseCalled()
	{
		// Arrange
		var configurationStackMock = new Mock<IConfigurationStack<IConverter>>(MockBehavior.Loose);

		// Act
		configurationStackMock.Object.UseFormattable();
		configurationStackMock.Object.UseFormattable("G");
		configurationStackMock.Object.UseFormattable(CultureInfo.InvariantCulture);
		configurationStackMock.Object.UseFormattable("G", CultureInfo.InvariantCulture);

		// Assert
		configurationStackMock
			.Verify(
				stack => stack.Use(It.IsAny<IJsonConverter>(), It.IsAny<bool>()),
				Times.Once
			);
		configurationStackMock
			.Verify(
				stack => stack.Use(It.IsAny<Func<IJsonConverter>>(), It.IsAny<bool>()),
				Times.Exactly(3)
			);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void UseFormattable_NullOrEmpty_Throws()
	{
		// Arrange
		var configurationStackMock = new Mock<IConfigurationStack<IConverter>>(MockBehavior.Loose);

		// Act
		var result1 = () => configurationStackMock.Object.UseFormattable(string.Empty);
		var result2 = () => configurationStackMock.Object.UseFormattable((string)null!);
		var result3 = () => configurationStackMock.Object.UseFormattable(string.Empty, CultureInfo.InvariantCulture);
		var result4 = () => configurationStackMock.Object.UseFormattable(null!, CultureInfo.InvariantCulture);

		// Assert
		result1.Should().ThrowExactly<ArgumentException>();
		result2.Should().ThrowExactly<ArgumentNullException>();
		result3.Should().ThrowExactly<ArgumentException>();
		result4.Should().ThrowExactly<ArgumentNullException>();
	}
}
