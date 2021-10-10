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
        public static bool EqualsTopLevel(this Type type, Type typeToEqual)
        {
            if (type.IsAssignableFrom(typeToEqual)) return true;
            if (!type.IsGenericType) return false;

            var genericType = type.GetGenericTypeDefinition();
            if (genericType.IsAssignableFrom(typeToEqual)) return true;
            if (!typeToEqual.IsGenericType) return false;

            var genericClassType = typeToEqual.GetGenericTypeDefinition();

            if (genericType.Equals(genericClassType)) return true;

            return genericType.IsAssignableFrom(genericClassType);
        }
        public static bool Implements(this Type type, Type interfaceType)
        {
            if (interfaceType.IsAssignableFrom(type)) return true;
            return type.GetInterfaces()
                .Any(typeInterface => typeInterface.IsGenericType
                && typeInterface.GetGenericTypeDefinition().Equals(interfaceType));
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

            throw new NotSupportedException($"Unable to create an enumerabble collection of '{type.FullName}'");
        }

        public static bool IsNullable(this PropertyInfo property) =>
            IsNullableHelper(property.PropertyType, property.DeclaringType, property.CustomAttributes);

        public static bool IsNullable(this FieldInfo field) =>
            IsNullableHelper(field.FieldType, field.DeclaringType, field.CustomAttributes);

        public static bool IsNullable(this ParameterInfo parameter) =>
            IsNullableHelper(parameter.ParameterType, parameter.Member, parameter.CustomAttributes);

        // todo cleanup
        private static bool IsNullableHelper(Type memberType, MemberInfo? declaringType, IEnumerable<CustomAttributeData> customAttributes)
        {
            if (memberType.IsValueType)
                return Nullable.GetUnderlyingType(memberType) != null;

            var nullable = customAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
            if (nullable != null && nullable.ConstructorArguments.Count == 1)
            {
                var attributeArgument = nullable.ConstructorArguments[0];
                if (attributeArgument.ArgumentType == typeof(byte[]))
                {
                    var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;
                    if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
                    {
                        return (byte)args[0].Value! == 2;
                    }
                }
                else if (attributeArgument.ArgumentType == typeof(byte))
                {
                    return (byte)attributeArgument.Value! == 2;
                }
            }

            for (var type = declaringType; type != null; type = type.DeclaringType)
            {
                var context = type.CustomAttributes
                    .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
                if (context != null &&
                    context.ConstructorArguments.Count == 1 &&
                    context.ConstructorArguments[0].ArgumentType == typeof(byte))
                {
                    return (byte)context.ConstructorArguments[0].Value! == 2;
                }
            }

            // Couldn't find a suitable attribute
            return false;
        }
    }
}