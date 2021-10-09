using FluentSerializer.Core.NamingStrategies;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FluentSerializer.Xml.Profiles
{
    public abstract class XmlSerializerProfile : IXmlSerializerProfile
    {
        private readonly Dictionary<Type, XmlClassMap> _classMaps = new Dictionary<Type, XmlClassMap>();
        public abstract void Configure();

        /// <remarks>
        /// Using an explicit interface here so it's not confusing to users of the <see cref="XmlSerializerProfile"/> but it's also not internal.
        /// </remarks>
        Dictionary<Type, XmlClassMap> IXmlSerializerProfile.Configure()
        {
            Configure();
            return _classMaps;
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

        protected XmlProfileBuilder<TModel> For<TModel>(
            INamingStrategy? tagNamingStrategy = null,
            INamingStrategy? attributeNamingStrategy = null)
            where TModel : new()
        {
            var classType = typeof(TModel);
            var propertymap = new List<XmlPropertyMap>();
            var builder = new XmlProfileBuilder<TModel>(
                attributeNamingStrategy ?? CamelCaseNamingStrategy,
                propertymap
            );

            _classMaps.Add(classType, new XmlClassMap(
                classType, 
                tagNamingStrategy ?? PascalCaseNamingStrategy,
                null, 
                propertymap
            ));

            return builder;
        }
    }
}
