using FluentSerializer.Core.Configuration;

using System;

namespace FluentSerializer.Core.Mapping;

/// <summary>
/// A collection of <see cref="IClassMap"/> with dedicated selection methods
/// </summary>
public interface IClassMapCollection
{
	/// <summary>
	/// Get a specific <see cref="IClassMap"/> matching <paramref name="type"/> and <paramref name="direction"/>
	/// </summary>
	IClassMap? GetClassMapFor(in Type type, in SerializerDirection direction);

	/// <summary>
	/// Get a specific <see cref="IClassMap"/> matching only <paramref name="type"/>
	/// </summary>
	IClassMap? GetClassMapFor(in Type type);
}