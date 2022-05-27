using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.Converting.Converters;

/// <summary>
/// Converts most dotnet collections
/// </summary>
public abstract class CollectionConverterBase : IConverter {

	/// <inheritdoc cref="IConverter.Direction" />
	public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc cref="IConverter.CanConvert(in Type)" />
	public virtual bool CanConvert(in Type targetType) => targetType.IsEnumerable();

	/// <inheritdoc />
	public int ConverterHashCode { get; } = typeof(IEnumerable).GetHashCode();

	/// <summary>
	/// Create a new instance of <see cref="IList"/> that matches the passed <paramref name="targetType"/> most closely
	/// </summary>
	/// <param name="targetType"></param>
	/// <returns></returns>
	/// <exception cref="NotSupportedException"></exception>
	/// <remarks>
	/// With types like T[] or other immutable collections, a <see cref="List{T}"/> will be generated
	/// and the consumer is responsible for calling <see cref="FinalizeEnumerableInstance(in IList?, in Type)"/>
	/// after building the collection;
	/// </remarks>
	protected static IList GetEnumerableInstance(in Type targetType)
	{
		if (targetType.Equals(typeof(IEnumerable)) && !targetType.IsGenericType) return new List<object>();
		if (targetType.Equals(typeof(IEnumerable))) return GenerateDefaultEnumerable(targetType);

		if (targetType.EqualsTopLevel(typeof(ArrayList))) return (IList)Activator.CreateInstance(targetType)!;
		if (targetType.EqualsTopLevel(typeof(List<>))) return (IList)Activator.CreateInstance(targetType)!;

		if (targetType.IsInterface && typeof(IEnumerable).IsAssignableFrom(targetType)) return GenerateDefaultEnumerable(targetType);
		if (TryCreateSystemArray(targetType, out var array)) return array;

		throw new NotSupportedException($"Unable to create an enumerable collection of '{targetType.FullName}'");
	}

	/// <summary>
	/// Convert an IList to the original requested type
	/// </summary>
	protected static IList? FinalizeEnumerableInstance(in IList? collection, in Type targetType)
	{
		if (collection is null) return null;

		if (targetType.Equals(typeof(ArrayList))) return new ArrayList(collection);
		if (targetType.IsArray) return ToArray(collection);
		return collection;
	}

	private static IList ToArray(IList list)
	{
		var genericType = list.GetType().GetGenericArguments()[0];
		var newArray = Array.CreateInstance(genericType, list.Count);

		list.CopyTo(newArray, 0);
		return newArray;
	}

	private static bool TryCreateSystemArray(Type type, out IList array)
	{
		array = Array.Empty<object>();

		if (type.BaseType?.IsAssignableFrom(typeof(Array)) != true) return false;
		if (!typeof(IEnumerable).IsAssignableFrom(type)) return false;

		Type? enumerableInterface = null;
		foreach (var typeInterface in type.GetInterfaces())
		{
			if (!typeInterface.IsGenericType) continue;
			if (!typeof(IList<>).IsAssignableFrom(typeInterface.GetGenericTypeDefinition())) continue;
			enumerableInterface = typeInterface;
			break;
		}

		if (enumerableInterface is null) return false;

		array = GenerateDefaultEnumerable(enumerableInterface);
		return true;
	}

	private static IList GenerateDefaultEnumerable(Type type)
	{
		var listType = typeof(List<>);
		var genericType = type.GetTypeInfo().GenericTypeArguments;
		return (IList)Activator.CreateInstance(listType.MakeGenericType(genericType))!;
	}
}