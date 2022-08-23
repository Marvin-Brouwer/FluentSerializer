using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming.NamingStrategies;

using Moq;

using System;
using System.Reflection;

namespace FluentSerializer.Core.Tests.Tests.Naming.NamingStrategies;

#pragma warning disable S101 // Types should be named in PascalCase
#pragma warning disable S2326 // Unused type parameters should be removed

public abstract class NamingStrategyTests
{
	protected abstract INamingStrategy Sut { get; }

	private static readonly Mock<INamingContext> NamingContextMock = new();

	public virtual void ValidString_GetName_ConvertsName(
		in Type typeInput, in PropertyInfo propertyInput,
		in string expectedClassName, in string expectedPropertyName)
	{
		// Act
		var typeResult = Sut.GetName(in typeInput, NamingContextMock.Object);
		var propertyResult = Sut.GetName(in propertyInput, propertyInput.PropertyType, NamingContextMock.Object);

		// Assert
		typeResult.ToString().Should().BeEquivalentTo(expectedClassName);
		propertyResult.ToString().Should().BeEquivalentTo(expectedPropertyName);
	}

	protected sealed class ClassNameWithMultipleParts
	{
		public int PropertyNameWithMultipleParts { get; set; }
	}

	protected sealed class ClassNameWith_strangeName
	{
		public int PropertyNameWith_strangeName { get; set; }
	}
	protected sealed class ClassNameWithGeneric<T>
	{
		public int Property { get; set; }
	}
}