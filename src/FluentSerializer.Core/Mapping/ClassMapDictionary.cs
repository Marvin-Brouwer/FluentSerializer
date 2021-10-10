using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Mapping
{
    public sealed class ClassMapDictionary : TypeDictionary<Type, IClassMap>
    {
        public ClassMapDictionary(IEnumerable<IClassMap> dataTypes) : base(dataTypes) { }

        protected override bool Compare(Type compareTo, IClassMap dataType)
        {
            if (!typeof(Nullable<>).IsAssignableFrom(compareTo))
                return compareTo.EqualsTopLevel(dataType.ClassType);

            var concreteCompareTo = compareTo.GetTypeInfo().GenericTypeArguments[0];
            return concreteCompareTo.EqualsTopLevel(dataType.ClassType);

        }
    }
}
