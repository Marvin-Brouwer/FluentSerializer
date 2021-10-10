using System;
using System.Collections;
using System.Collections.Generic;
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
            if (typeof(Array).IsAssignableFrom(type)) return (IList)Activator.CreateInstance(type)!;
            if (typeof(ArrayList).IsAssignableFrom(type)) return (IList)Activator.CreateInstance(type)!;
            if (typeof(List<>).IsAssignableFrom(type)) return (IList)Activator.CreateInstance(type)!;

            if (type.IsInterface && type.IsAssignableFrom(typeof(IEnumerable<>)))
            {
                var listType = typeof(List<>);
                var genericType = type.GetTypeInfo().GenericTypeArguments;
                return (IList)Activator.CreateInstance(listType.MakeGenericType(genericType))!;
            }

            throw new NotSupportedException($"Unable to create an enumerabble collection of '{type.FullName}'");
        }
    }
}