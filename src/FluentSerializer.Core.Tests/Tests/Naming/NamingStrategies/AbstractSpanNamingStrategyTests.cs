using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming.NamingStrategies;

using Moq;

using System;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Naming.NamingStrategies;

public sealed class AbstractSpanNamingStrategyTests : NamingStrategyTests
{
	protected override INamingStrategy Sut => new TestNamingStrategy("&:illegal <name>");

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetName_ConvertsToInvalidString_Throws()
	{
		// Arrange
		var namingContextMock = new Mock<INamingContext>();
		var typeInput = typeof(ClassNameWithMultipleParts);
		var propertyInput = typeInput.GetProperty(nameof(ClassNameWithMultipleParts.PropertyNameWithMultipleParts))!;
		
		// Act
		var typeResult = () => Sut.GetName(in typeInput, namingContextMock.Object).ToString();
		var propertyResult = () => Sut.GetName(in propertyInput, propertyInput.PropertyType, namingContextMock.Object).ToString();

		// Assert
		typeResult.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName("newName");
		propertyResult.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName("newName");
	}

	private sealed class TestNamingStrategy : AbstractSpanNamingStrategy
	{
		private readonly string _newName;

		public TestNamingStrategy(string newName)
		{
			_newName = newName;
		}

		protected override void ConvertCasing(in ReadOnlySpan<char> sourceSpan, ref Span<char> characterSpan)
		{
			_newName.CopyTo(characterSpan);
		}
	}
}