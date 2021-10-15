using FluentSerializer.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.Profiles;

namespace FluentSerializer.Xml.Profiles
{
    /// <summary>
    /// This attribute purely exists to indicate that this class is not an unused class
    /// The reason that we don't use the ReSharper version is that we can't assume people will be using ReSharper, hence we don't depend on their NuGet.
    /// This still works because libraries like ReSharper will check if the class is a JsonProperty or an XmlElement which this attribute inherits from.
    /// Since <see cref="XmlElementAttribute"/> is a system type, no additional dependencies are required.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    [System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerStepThrough]
    internal sealed class ImplicitlyUsedAttribute : XmlElementAttribute { }
    
    [ImplicitlyUsed]
    public abstract class XmlSerializerProfile : ISerializerProfile
    {
        private readonly List<IClassMap> _classMaps = new List<IClassMap>();
        
        protected abstract void Configure();

        /// <remarks>
        /// Using an explicit interface here so it's not confusing to users of the <see cref="XmlSerializerProfile"/> but it's also not internal.
        /// </remarks>
        [System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerStepThrough, 
         System.Diagnostics.DebuggerHidden]
        IReadOnlyList<IClassMap> ISerializerProfile.Configure()
        {
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
                attributeNamingStrategy ?? Names.Use.CamelCase,
                propertyMap
            );

            // Store in a tuple for lazy evaluation
            _classMaps.Add(new ClassMap(
                classType, 
                direction,
                tagNamingStrategy ?? Names.Use.PascalCase, 
                propertyMap.AsReadOnly()));

            return builder;
        }
    }
}
