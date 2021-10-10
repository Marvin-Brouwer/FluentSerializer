using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Xml.Converters;
using FluentSerializer.Xml.Mapping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FluentSerializer.Xml.Profiles
{
    public abstract class XmlSerializerProfile : IXmlSerializerProfile
    {
        private readonly List<(Type, INamingStrategy, IEnumerable<IPropertyMap>)> _classMaps = new List<(Type, INamingStrategy, IEnumerable<IPropertyMap>)>();
        public abstract void Configure();

        /// <remarks>
        /// Using an explicit interface here so it's not confusing to users of the <see cref="XmlSerializerProfile"/> but it's also not internal.
        /// </remarks>
        IEnumerable<XmlClassMap> IXmlSerializerProfile.Configure()
        {
            Configure();
            return _classMaps.Select(lazyClassMap => new XmlClassMap(lazyClassMap));
        }

        protected CustomNamingStrategy CustomNamingStrategy(string nameOverride) => new CustomNamingStrategy(nameOverride);
        protected readonly LowerCaseNamingStrategy LowerCaseNamingStrategy = new LowerCaseNamingStrategy();
        protected readonly CamelCaseNamingStrategy CamelCaseNamingStrategy = new CamelCaseNamingStrategy();
        protected readonly PascalCaseNamingStrategy PascalCaseNamingStrategy = new PascalCaseNamingStrategy();
        protected readonly SnakeCaseNamingStrategy SnakeCaseNamingStrategy = new SnakeCaseNamingStrategy();
        protected readonly KebabCaseNamingStrategy KebabCaseNamingStrategy = new KebabCaseNamingStrategy();

        protected DateByFormatConverter DateByFormatConverter(
            string format, CultureInfo? cultureInfo = null, DateTimeStyles? dateTimeStyle = null) =>
            new DateByFormatConverter(format, cultureInfo ?? CultureInfo.CurrentCulture, dateTimeStyle ?? DateTimeStyles.None);
        
        protected static readonly NonWrappedCollectionConverter NonWrappedCollectionConverter = 
            new NonWrappedCollectionConverter();

        protected XmlProfileBuilder<TModel> For<TModel>(
            INamingStrategy? tagNamingStrategy = null,
            INamingStrategy? attributeNamingStrategy = null)
            where TModel : new()
        {
            var classType = typeof(TModel);
            var propertyMap = new List<XmlPropertyMap>();
            var builder = new XmlProfileBuilder<TModel>(
                attributeNamingStrategy ?? CamelCaseNamingStrategy,
                propertyMap
            );

            // Store in a tuple for lazy evaluation
            _classMaps.Add((
                classType, 
                tagNamingStrategy ?? PascalCaseNamingStrategy, 
                propertyMap));

            return builder;
        }
    }
}
