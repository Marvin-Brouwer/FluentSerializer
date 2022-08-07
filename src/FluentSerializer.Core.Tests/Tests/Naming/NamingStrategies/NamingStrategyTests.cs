using FluentAssertions;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Naming.NamingStrategies;

public abstract class NamingStrategyTests
{
	protected abstract INamingStrategy Sut { get; }
	protected abstract INewNamingStrategy SutNew { get; }

	private static readonly Mock<INamingContext> _namingContextMock = new();

	public virtual void ValidString_GetName_ConvertsName(
		in Type typeInput, in PropertyInfo propertyInput,
		in string expectedClassName, in string expectedPropertyName)
	{
		// Act
		var typeResult = Sut.GetName(in typeInput, _namingContextMock.Object);
		var propertyResult = Sut.GetName(in propertyInput, propertyInput.PropertyType, _namingContextMock.Object);

		// Assert
		typeResult.Should().BeEquivalentTo(expectedClassName);
		propertyResult.Should().BeEquivalentTo(expectedPropertyName);
	}

	public virtual void ValidString_GetName_ConvertsName_New(
		in Type typeInput, in PropertyInfo propertyInput,
		in string expectedClassName, in string expectedPropertyName)
	{
		// Act
		var typeResult = SutNew.GetName(in typeInput, _namingContextMock.Object);
		var propertyResult = SutNew.GetName(in propertyInput, propertyInput.PropertyType, _namingContextMock.Object);

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