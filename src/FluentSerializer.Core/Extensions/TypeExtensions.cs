using Ardalis.GuardClauses;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Core.Extensions;

/// <summary>
/// Extension methods to help reflecting additional type information
/// </summary>
public static class TypeExtensions
{
	private const int NullableArgumentType = 2;
	private const string NullableAttributeName = "System.Runtime.CompilerServices.NullableAttribute";
	private const string NullableContextAttributeName = "System.Runtime.CompilerServices.NullableContextAttribute";

	/// <summary>
	/// Check whether types equal, ignoring generics by pretending they are open generic types
	/// </summary>
	public static bool EqualsTopLevel(this Type type, in Type typeToEqual)
	{
		Guard.Against.Null(type
#if NETSTANDARD2_1
			, nameof(type)
#endif
		);
		Guard.Against.Null(typeToEqual
#if NETSTANDARD2_1
			, nameof(typeToEqual)
#endif
		);

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
		Guard.Against.Null(type
#if NETSTANDARD2_1
			, nameof(type)
#endif
		);
		Guard.Against.Null(interfaceType
#if NETSTANDARD2_1
			, nameof(interfaceType)
#endif
		);

		if (interfaceType.IsAssignableFrom(type)) return true;
		return type.GetInterfaces()
			.Any(typeInterface => typeInterface.IsGenericType && typeInterface.GetGenericTypeDefinition().Equals(interfaceType));
	}

	/// <summary>
	/// Check if a type is Enumerable but not a string.
	/// </summary>
	public static bool IsEnumerable(this Type type)
	{
		Guard.Against.Null(type
#if NETSTANDARD2_1
			, nameof(type)
#endif
		);

		return !typeof(string).IsAssignableFrom(type)
		     && type.Implements(typeof(IEnumerable));
	}

	/// <inheritdoc cref="IsNullable(Type)"/>
	public static bool IsNullable(this PropertyInfo property)
	{
		Guard.Against.Null(property
#if NETSTANDARD2_1
			, nameof(property)
#endif
		);

		return CheckIfNullable(property.PropertyType, property.DeclaringType, property.CustomAttributes);
	}

	/// <inheritdoc cref="IsNullable(Type)"/>
	public static bool IsNullable(this FieldInfo field)
	{
		Guard.Against.Null(field
#if NETSTANDARD2_1
			, nameof(field)
#endif
		);

		return CheckIfNullable(field.FieldType, field.DeclaringType, field.CustomAttributes);
	}

	/// <inheritdoc cref="IsNullable(Type)"/>
	public static bool IsNullable(this ParameterInfo parameter)
	{
		Guard.Against.Null(parameter
#if NETSTANDARD2_1
			, nameof(parameter)
#endif
		);

		return CheckIfNullable(parameter.ParameterType, parameter.Member, parameter.CustomAttributes);
	}

	/// <summary>
	/// Check whether this type is attributed to allow null values
	/// </summary>
	public static bool IsNullable(this Type type)
	{
		Guard.Against.Null(type
#if NETSTANDARD2_1
			, nameof(type)
#endif
		);

		return CheckIfNullable(type, null, type.CustomAttributes);
	}

	private static bool CheckIfNullable(in Type memberType, in MemberInfo? declaringType, in IEnumerable<CustomAttributeData> customAttributes)
	{
		var typeToCheck = memberType.IsByRef ? memberType.GetElementType()! : memberType;

		if (typeToCheck.IsValueType)
			return Nullable.GetUnderlyingType(typeToCheck) != null;

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
		if (nullableCompilerServiceAttribute is null) return false;
		if (nullableCompilerServiceAttribute.ConstructorArguments.Count != 1) return false;

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