using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.Extensions;

using Moq;

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
				stack => stack.Use(It.IsAny<EnumConverter>(), false),
				Times.Once
			);
	}
}
