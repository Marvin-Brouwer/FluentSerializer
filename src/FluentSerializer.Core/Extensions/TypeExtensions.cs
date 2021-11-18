using Ardalis.GuardClauses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Core.Extensions
{
    public static class TypeExtensions
    {
        private const int NullableArgumentType = 2;
        private const string NullableAttributeName = "System.Runtime.CompilerServices.NullableAttribute";
        private const string NullableContextAttributeName = "System.Runtime.CompilerServices.NullableContextAttribute";

        public static bool EqualsTopLevel(this Type type, Type typeToEqual)
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

        public static bool Implements(this Type type, Type interfaceType)
        {
            if (interfaceType.IsAssignableFrom(type)) return true;
            return type.GetInterfaces()
                .Any(typeInterface => typeInterface.IsGenericType && typeInterface.GetGenericTypeDefinition() == interfaceType);
        }

        public static IList GetEnumerableInstance(this Type type)
        {
            if (type.EqualsTopLevel(typeof(Array))) return (IList)Activator.CreateInstance(type)!;
            if (type.EqualsTopLevel(typeof(ArrayList))) return (IList)Activator.CreateInstance(type)!;
            if (type.EqualsTopLevel(typeof(List<>))) return (IList)Activator.CreateInstance(type)!;

            if (type.IsInterface && type.IsAssignableFrom(typeof(IEnumerable<>)))
            {
                var listType = typeof(List<>);
                var genericType = type.GetTypeInfo().GenericTypeArguments;
                return (IList)Activator.CreateInstance(listType.MakeGenericType(genericType))!;
            }

            throw new NotSupportedException($"Unable to create an enumerable collection of '{type.FullName}'");
        }
        public static bool IsEnumerable(this Type type) => 
            !typeof(string).IsAssignableFrom(type) &&
            type.Implements(typeof(IEnumerable));

        public static bool IsNullable(this PropertyInfo property) =>
            IsNullableHelper(property.PropertyType, property.DeclaringType, property.CustomAttributes);

        public static bool IsNullable(this FieldInfo field) =>
            IsNullableHelper(field.FieldType, field.DeclaringType, field.CustomAttributes);
        
        public static bool IsNullable(this ParameterInfo parameter) =>
            IsNullableHelper(parameter.ParameterType, parameter.Member, parameter.CustomAttributes);

        public static bool IsNullable(this Type type) =>
            IsNullableHelper(type, null, type.CustomAttributes);

        private static bool IsNullableHelper(Type memberType, MemberInfo? declaringType, IEnumerable<CustomAttributeData> customAttributes)
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
}