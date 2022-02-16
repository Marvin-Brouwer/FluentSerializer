using Ardalis.GuardClauses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Core.Extensions;

/// <summary>
/// Extension methods to help reflecting addtional type information
/// </summary>
public static class TypeExtensions
{
	private const int NullableArgumentType = 2;
	private const string NullableAttributeName = "System.Runtime.CompilerServices.NullableAttribute";
	private const string NullableContextAttributeName = "System.Runtime.CompilerServices.NullableContextAttribute";

	/// <summary>
	/// Check wheter types equal, ignoring generics by pretending they are open generic types
	/// </summary>
	public static bool EqualsTopLevel(this Type type, in Type typeToEqual)
	{
		Guard.Against.Null(typeToEqual, nameof(typeToEqual));

		if (type.IsAssignableFrom(typeToEqual)) return true;
		if (!type.IsGenericType) return false;

		var genericType = type.GetGenericTypeDefinition();
		if (genericType.IsAssignableFrom(typeToEqual)) return true;
		if (!typeToEqual.IsGenericType) return false;

		var genericClassType = typeToEqual.GetGenericTypeDefinition();

		if (genericType == genericClassType) return true;

		return genericType.IsAssignableFrom(genericClassType);
	}

	/// <summary>
	/// Check whether a type implements an interface of type
	/// </summary>
	public static bool Implements(this Type type, Type interfaceType)
	{
		if (interfaceType.IsAssignableFrom(type)) return true;
		return type.GetInterfaces()
			.Any(typeInterface => typeInterface.IsGenericType && typeInterface.GetGenericTypeDefinition().Equals(interfaceType));
	}

	/// <summary>
	/// Create a new instance of <see cref="IList"/> that matches the passed <paramref name="type"/> most closely
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	/// <exception cref="NotSupportedException"></exception>
	/// <remarks>
	/// With type of T[] a <see cref="List{T}"/> will be generated and the consumer is responsible for
	/// calling <see cref="Enumerable.ToArray{TSource}(IEnumerable{TSource})"/> after building the array;
	/// </remarks>
	public static IList GetEnumerableInstance(this Type type)
	{
		if (type.Equals(typeof(IEnumerable)) && !type.IsGenericType) return new List<object>();
		if (type.Equals(typeof(IEnumerable))) return GenerateDefaultEnumerable(type);

		if (type.EqualsTopLevel(typeof(ArrayList))) return (IList)Activator.CreateInstance(type)!;
		if (type.EqualsTopLevel(typeof(List<>))) return (IList)Activator.CreateInstance(type)!;

		if (type.IsInterface && typeof(IEnumerable).IsAssignableFrom(type)) return GenerateDefaultEnumerable(type);
		if (TryCreateSystemArray(type, out var array)) return array;

		throw new NotSupportedException($"Unable to create an enumerable collection of '{type.FullName}'");
	}

	/// <summary>
	/// Convert an IList to the original one dimensional array type
	/// </summary>
	public static IList ToArray(this IList list)
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
		foreach (var typeInterface in type.GetInterfaces()){
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

	/// <summary>
	/// Check if a type is Enumerable but not a string.
	/// </summary>
	public static bool IsEnumerable(this Type type) => 
		!typeof(string).IsAssignableFrom(type) &&
		type.Implements(typeof(IEnumerable));

	/// <inheritdoc cref="IsNullable(Type)"/>
	public static bool IsNullable(this PropertyInfo property) =>
		IsNullableHelper(property.PropertyType, property.DeclaringType, property.CustomAttributes);
	/// <inheritdoc cref="IsNullable(Type)"/>
	public static bool IsNullable(this FieldInfo field) =>
		IsNullableHelper(field.FieldType, field.DeclaringType, field.CustomAttributes);
    /// <inheritdoc cref="IsNullable(Type)"/>
	public static bool IsNullable(this ParameterInfo parameter) =>
		IsNullableHelper(parameter.ParameterType, parameter.Member, parameter.CustomAttributes);
	/// <summary>
	/// Check whether this type is attributed to allow null values
	/// </summary>
	public static bool IsNullable(this Type type) =>
		IsNullableHelper(in type, null, type.CustomAttributes);

	private static bool IsNullableHelper(in Type memberType, in MemberInfo? declaringType, in IEnumerable<CustomAttributeData> customAttributes)
	{
		if (memberType.IsValueType)
			return Nullable.GetUnderlyingType(memberType) != null;

		if (HasNullableAttribute(customAttributes)) return true;
		if (HasNullableContextAttribute(declaringType)) return true;

		// Couldn't find a suitable attribute
		return false;
	}

	private static bool HasNullableContextAttribute(MemberInfo? declaringType)
	{
		if (declaringType is null) return false;

		for (var type = declaringType; type != null; type = type.DeclaringType)
		{
			// Use full name so any runtime will pass
			var context = type.CustomAttributes
				.FirstOrDefault(attribute => attribute.AttributeType.FullName == NullableContextAttributeName);
			if (context is null) continue;
                
			if (context.ConstructorArguments.Count == 1 && context.ConstructorArguments[0].ArgumentType == typeof(byte))
				return (byte)context.ConstructorArguments[0].Value! == NullableArgumentType;
		}

		return false;
	}

	private static bool HasNullableAttribute(IEnumerable<CustomAttributeData> customAttributes)
	{
		// Use full name so any runtime will pass
		var nullableCompilerServiceAttribute = customAttributes
			.FirstOrDefault(attribute => attribute.AttributeType.FullName == NullableAttributeName);
		if (nullableCompilerServiceAttribute?.ConstructorArguments.Count != 1) return false;

		var attributeArgument = nullableCompilerServiceAttribute.ConstructorArguments[0];
		if (attributeArgument.ArgumentType == typeof(byte[]))
		{
			var attributeArguments = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;
			if (attributeArguments.Count > 0 
			    && attributeArguments[0].ArgumentType == typeof(byte)) return (byte)attributeArguments[0].Value! == NullableArgumentType;
		}

		if (attributeArgument.ArgumentType == typeof(byte))
			return (byte)attributeArgument.Value! == NullableArgumentType;

		return false;
	}
}