using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.Converting.Converters;

/// <summary>
/// Converts most dotnet collections
/// </summary>
public abstract class CollectionConverterBase : IConverter {

	private static MethodInfo? _immutableArrayMethod;
	private static MethodInfo? _immutableListMethod;

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
		if (targetType.IsGenericType && targetType.EqualsTopLevel(typeof(ImmutableList<>)))
			return CreateImmutableListBuilder(in targetType);
		if (targetType.IsGenericType && targetType.EqualsTopLevel(typeof(ImmutableArray<>)))
			return CreateImmutableListBuilder(in targetType);

		if (targetType == typeof(IEnumerable) && !targetType.IsGenericType) return new List<object>();
		if (targetType == typeof(IEnumerable)) return GenerateDefaultEnumerable(in targetType);

		if (targetType.EqualsTopLevel(typeof(ArrayList))) return (IList)Activator.CreateInstance(targetType)!;
		if (targetType.EqualsTopLevel(typeof(List<>))) return (IList)Activator.CreateInstance(targetType)!;
		if (targetType.IsArray) return GenerateDefaultEnumerable(in targetType);

		// All enumerable fallback
		if (targetType.IsInterface && typeof(IEnumerable).IsAssignableFrom(targetType))
			return CreateImmutableListBuilder(in targetType);

		throw new NotSupportedException($"Unable to create an enumerable collection of '{targetType.FullName}'");
	}

	/// <summary>
	/// Convert an IList to the original requested type
	/// </summary>
	protected static IEnumerable? FinalizeEnumerableInstance(in IList? collection, in Type targetType)
	{
		if (collection is null) return null;

		if (targetType.IsGenericType && targetType.EqualsTopLevel(typeof(ImmutableList<>)))
			return ToImmutableList(in collection, in targetType);
		if (targetType.IsGenericType && targetType.EqualsTopLevel(typeof(ImmutableArray<>)))
			return ToImmutableArray(in collection, in targetType);

		if (targetType == typeof(ArrayList)) return new ArrayList(collection);
		if (targetType.IsArray) return ToArray(in collection, in targetType);

		if (targetType.IsInterface && typeof(IEnumerable).IsAssignableFrom(targetType))
			return ToImmutableArray(in collection, collection.GetType());

		return collection;
	}

	private static IEnumerable ToImmutableList(in IList collection, in Type targetType)
	{
		var genericTypes = GetGenericTypeInfo(in targetType);
		
		if (_immutableListMethod is null)
			foreach (var method in typeof(ImmutableList).GetMethods(
				         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
			{
				if (!method.Name.Equals(nameof(ImmutableList.ToImmutableList), StringComparison.Ordinal)) continue;
				if (typeof(IEnumerable).IsAssignableFrom(method.GetParameters()[0].ParameterType))
				{
					_immutableListMethod = method;
					break;
				}
			}

		var genericMethod = _immutableListMethod!.MakeGenericMethod(genericTypes);
		return (IEnumerable)genericMethod.Invoke(null, new object?[] { collection })!;
	}

	private static IEnumerable ToImmutableArray(in IList collection, in Type targetType)
	{
		var genericTypes = GetGenericTypeInfo(targetType);

		if (_immutableArrayMethod is null)
			foreach (var method in typeof(ImmutableArray).GetMethods(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
			{
				if (!method.Name.Equals(nameof(ImmutableArray.ToImmutableArray), StringComparison.Ordinal)) continue;
				if (typeof(IEnumerable).IsAssignableFrom(method.GetParameters()[0].ParameterType))
				{
					_immutableArrayMethod = method;
					break;
				}
			}

		var genericMethod = _immutableArrayMethod!.MakeGenericMethod(genericTypes);
		return (IEnumerable)genericMethod.Invoke(null, new object?[] { collection })!;
	}

	private static IList ToArray(in IList list, in Type targetType)
	{
		var genericTypes = GetGenericTypeInfo(in targetType);
		var newArray = Array.CreateInstance(genericTypes[0], list.Count);

		list.CopyTo(newArray, 0);
		return newArray;
	}

	private static IList GenerateDefaultEnumerable(in Type targetType)
	{
		var listType = typeof(List<>);
		var genericTypes = GetGenericTypeInfo(in targetType);

		return (IList)Activator.CreateInstance(listType.MakeGenericType(genericTypes))!;
	}

	private static IList CreateImmutableListBuilder(in Type targetType)
	{
		var genericTypes = GetGenericTypeInfo(in targetType);

		var method = typeof(ImmutableList).GetMethod(nameof(ImmutableList.CreateBuilder));
		var genericMethod = method!.MakeGenericMethod(genericTypes);
		return (IList)genericMethod.Invoke(null, null)!;
	}

	private static Type[] GetGenericTypeInfo(in Type targetType)
	{
		if (!typeof(IEnumerable).IsAssignableFrom(targetType)) throw new NotSupportedException("Not enumerable");
		if (typeof(Array).IsAssignableFrom(targetType.BaseType)) return GetArrayTypeInfo(in targetType);

		var genericTypes = targetType.GetTypeInfo().GenericTypeArguments;
		if (genericTypes.Length == 0) genericTypes = new[] { typeof(object) };

		return genericTypes;
	}

	private static Type[] GetArrayTypeInfo(in Type targetType)
	{
		foreach (var typeInterface in targetType.GetInterfaces())
		{
			if (!typeInterface.IsGenericType) continue;
			if (!typeof(IList<>).IsAssignableFrom(typeInterface.GetGenericTypeDefinition())) continue;

			return GetGenericTypeInfo(in typeInterface);
		}

		return new [] { typeof(object) };
	}
}