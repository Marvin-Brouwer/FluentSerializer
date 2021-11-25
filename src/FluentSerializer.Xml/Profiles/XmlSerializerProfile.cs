using FluentSerializer.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Xml.Configuration;

namespace FluentSerializer.Xml.Profiles
{
    [ImplicitlyUsed]
    public abstract class XmlSerializerProfile : ISerializerProfile
    {
        private readonly List<IClassMap> _classMaps = new();
        private XmlSerializerConfiguration _configuration = XmlSerializerConfiguration.Default;

        protected abstract void Configure();

        /// <remarks>
        /// Using an explicit interface here so it's not confusing to users of the <see cref="XmlSerializerProfile"/> but it's also not internal.
        /// </remarks>
        [System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerStepThrough, 
         System.Diagnostics.DebuggerHidden]
        IReadOnlyList<IClassMap> ISerializerProfile.Configure(SerializerConfiguration configuration)
        {
            _configuration = (XmlSerializerConfiguration)configuration;
            Configure();
            return new ReadOnlyCollection<IClassMap>(_classMaps);
        }
        
        protected IXmlProfileBuilder<TModel> For<TModel>(
            SerializerDirection direction = SerializerDirection.Both,
            Func<INamingStrategy>? tagNamingStrategy = null,
            Func<INamingStrategy>? attributeNamingStrategy = null)
            where TModel : new()
        {
            var classType = typeof(TModel);
            var propertyMap = new List<IPropertyMap>();
            var builder = new XmlProfileBuilder<TModel>(
                attributeNamingStrategy ?? _configuration.DefaultPropertyNamingStrategy,
                propertyMap
            );

            // Store in a tuple for lazy evaluation
            _classMaps.Add(new ClassMap(
                classType, 
                direction,
                tagNamingStrategy ?? _configuration.DefaultClassNamingStrategy, 
                propertyMap.AsReadOnly()));

            return builder;
        }
    }
}
