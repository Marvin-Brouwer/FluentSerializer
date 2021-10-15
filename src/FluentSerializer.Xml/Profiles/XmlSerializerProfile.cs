using FluentSerializer.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.Profiles;

namespace FluentSerializer.Xml.Profiles
{
    public abstract class XmlSerializerProfile : ISerializerProfile
    {
        private readonly List<(Type classType, Func<INamingStrategy> namingStrategy, IEnumerable<IPropertyMap> propertyMap)> _classMaps = new List<(Type, Func<INamingStrategy>, IEnumerable<IPropertyMap>)>();
        public abstract void Configure();

        /// <remarks>
        /// Using an explicit interface here so it's not confusing to users of the <see cref="XmlSerializerProfile"/> but it's also not internal.
        /// </remarks>
        IEnumerable<IClassMap> ISerializerProfile.Configure()
        {
            Configure();
            return _classMaps.Select(lazyClassMap => new ClassMap(
                lazyClassMap.classType, lazyClassMap.namingStrategy, lazyClassMap.propertyMap
            ));
        }

        protected IXmlProfileBuilder<TModel> For<TModel>(
            Func<INamingStrategy>? tagNamingStrategy = null,
            Func<INamingStrategy>? attributeNamingStrategy = null)
            where TModel : new()
        {
            var classType = typeof(TModel);
            var propertyMap = new List<IPropertyMap>();
            var builder = new XmlProfileBuilder<TModel>(
                attributeNamingStrategy ?? Names.Use.CamelCase,
                propertyMap
            );

            // Store in a tuple for lazy evaluation
            _classMaps.Add((
                classType, 
                tagNamingStrategy ?? Names.Use.PascalCase, 
                propertyMap));

            return builder;
        }
    }
}
