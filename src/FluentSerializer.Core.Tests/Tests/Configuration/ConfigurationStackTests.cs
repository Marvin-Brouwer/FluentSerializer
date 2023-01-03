using FluentAssertions;

using FluentSerializer.Core.Comparing;
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
/// Effectively meaning, that new <see cref"Core.Converting.IConverter"/>s will be added to the top, existing items will only be updated.
/// But, still allowing to force overloads for system types / OOTB converters to be repositioned with the forceTop option.
/// </remarks>
public sealed class ConfigurationStackTests
{
	private sealed class TestComparer : IEqualityComparer<int>
	{
		public bool Equals(int x, int y) => DefaultReferenceComparer.Default.Equals(x, y);

		public int GetHashCode(int obj) => DefaultReferenceComparer.Default.GetHashCode(obj);
	}

	private static ConfigurationStack<int> Sut => new(new TestComparer(),
		1, 2, 3
	);

	[Fact]
	public void Use_ExistingItem_NotAdded()
	{
		// Arrange
		const int item = 2;
		var expected = new[] { 3, 2, 1 };

		// Act
		var result = Sut.Use(item).ToList();
		var funcResult = Sut.Use(() => item).ToList();

		// Assert
		result.Should().BeEquivalentTo(expected,
			config => config.WithStrictOrdering()
		);
		funcResult.Should().BeEquivalentTo(expected,
			config => config.WithStrictOrdering()
		);
	}

	[Fact]
	public void Use_ExistingItem_ForceTop_MovedToTop()
	{
		// Arrange
		const int item = 2;
		var expected = new[] { 2, 3, 1 };

		// Act
		var result = Sut.Use(item, true).ToList();
		var funcResult = Sut.Use(() => item, true).ToList();

		// Assert
		result.Should().BeEquivalentTo(expected,
			config => config.WithStrictOrdering()
		);
		funcResult.Should().BeEquivalentTo(expected,
			config => config.WithStrictOrdering()
		);
	}

	[Fact]
	public void Use_NewItem_AddedToTop()
	{
		// Arrange
		const int item = 4;
		var expected = new[] { 4, 3, 2, 1 };

		// Act
		var result = Sut.Use(item).ToList();
		var funcResult = Sut.Use(() => item).ToList();

		// Assert
		result.Should().BeEquivalentTo(expected,
			config => config.WithStrictOrdering()
		);
		funcResult.Should().BeEquivalentTo(expected,
			config => config.WithStrictOrdering()
		);
	}

	[Fact]
	public void Use_NewItem_ForceTop_AddedToTop()
	{
		// Arrange
		const int item = 4;
		var expected = new[] { 4, 3, 2, 1 };

		// Act
		var result = Sut.Use(item, true).ToArray();
		var funcResult = Sut.Use(() => item, true).ToList();

		// Assert
		result.Should().BeEquivalentTo(expected,
			config => config.WithStrictOrdering()
		);
		funcResult.Should().BeEquivalentTo(expected,
			config => config.WithStrictOrdering()
		);
	}

	[Fact]
	public void GetEnumerator_ShouldBeReversed()
	{
		// Arrange
		var sut = new ConfigurationStack<int>(new TestComparer(),
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9
		);
		var expected = new[]
		{
			9, 8, 7, 6, 5, 4, 3, 2, 1, 0
		};

		// Act
		using var enumerator = sut.GetEnumerator();

		// Assert
		Enumerate(enumerator).Should().BeEquivalentTo(expected,
			config => config.WithStrictOrdering()
		);

		static IEnumerable<T> Enumerate<T>(IEnumerator<T> enumerator)
		{
			while (enumerator.MoveNext())
			{
				yield return enumerator.Current;
			}
		}
	}
}
