using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public abstract class ClassMap<TDestination, TPropertyDestination, TPropertyMap> : IClassMap<TDestination, TPropertyDestination, TPropertyMap> 
        where TDestination : class
        where TPropertyDestination : class
        where TPropertyMap : class, IPropertyMap<TPropertyDestination>
    {
        public ClassMap(
            Type classType,
            INamingStrategy namingStrategy,
            IConverter<TDestination>? customConverter,
            List<TPropertyMap> propertyMap)
        {
            ClassType = classType;
            NamingStrategy = namingStrategy;
            CustomConverter = customConverter;
            PropertyMap = propertyMap.ToLookup(x => x.Property, x => x);
        }
        public Type ClassType { get; }
        public INamingStrategy NamingStrategy { get; }
        // TODO see if a custom converter on class level is useful?
        public IConverter<TDestination>? CustomConverter { get; }

        public ILookup<PropertyInfo, TPropertyMap> PropertyMap { get; }
    }
}
