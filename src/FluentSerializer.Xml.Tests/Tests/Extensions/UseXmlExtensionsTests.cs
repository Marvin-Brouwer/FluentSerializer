using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.Extensions;

using Moq;

using Xunit;

namespace FluentSerializer.Xml.Tests.Tests.Extensions;

public sealed class UseXmlExtensionsTests
{
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
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
