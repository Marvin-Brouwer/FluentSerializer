using FluentAssertions;

using FluentSerializer.Core.Configuration;

using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Configuration;

/// <summary>
/// These tests are to illustrate how a <see cref="ConfigurationStack{T}"/> operates.
/// Basically you have a base state you can set in the type initializer.
/// From that point on you can call <see cref="ConfigurationStack{T}.Use(T, bool)"/> with forceTop set to <c>true</c> or <c>false</c>(/omitting).
/// Calling use will act like a <see cref="HashSet{T}"/> making sure items are added only once.
/// Enumerating the <see cref="ConfigurationStack{T}"/> will reverse the Stack so newer items are added later.
/// </summary>
/// <remarks>
/// Functionally, this will be used for configuring <see cref"Core.Converting.IConverter"/>s.
/// Effectively meaning, that new  <see cref"Core.Converting.IConverter"/>s will be lower in the chain for custom types.
/// But, still allowing to force overloads for system types / OOTB converters to be overridden with the forceTop option.
/// </remarks>
public sealed class ConfigurationStackTests
{
	private sealed class TestComparer : IComparer<int>
	{
		public int Compare(int x, int y) => x.CompareTo(y);
	}
	private static ConfigurationStack<int> Sut => new(new TestComparer())
	{
		1, 2, 3
	};

	[Fact]
	public void Use_ExistingItem_NotAdded()
	{
		// Arrange
		var item = 2;

		// Act
		var result = Sut.Use(item);

		// Assert
		result.AsEnumerable().Should().BeEquivalentTo(new List<int>
		{
			3, 2, 1
		});
	}

	[Fact]
	public void Use_ExistingItem_ForceTop_MovedToTop()
	{
		// Arrange
		var item = 2;

		// Act
		var result = Sut.Use(item, true);

		// Assert
		result.AsEnumerable().Should().BeEquivalentTo(new List<int>
		{
			2, 3, 1
		});
	}

	[Fact]
	public void Use_NewItem_AddedToBottom()
	{
		// Arrange
		var item = 4;

		// Act
		var result = Sut.Use(item);

		// Assert
		result.AsEnumerable().Should().BeEquivalentTo(new List<int>
		{
			3, 2, 1, 4
		});
	}

	[Fact]
	public void Use_NewItem_ForceTop_AddedToTop()
	{
		// Arrange
		var item = 4;

		// Act
		var result = Sut.Use(item, true);

		// Assert
		result.AsEnumerable().Should().BeEquivalentTo(new List<int>
		{
			4, 3, 2, 1
		});
	}
}
