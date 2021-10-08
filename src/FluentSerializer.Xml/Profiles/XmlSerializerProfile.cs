using FluentSerializer.Xml.Profiles;
using FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles;
using System.Globalization;

namespace FluentSerializer.Xml.Configuration
{
    public abstract class XmlSerializerProfile
    {
        public abstract void Configure();

        // todo add namingstrategies
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
            where TModel : new() =>
            new XmlProfileBuilder<TModel>(
                tagNamingStrategy ?? PascalCaseNamingStrategy,
                attributeNamingStrategy ?? CamelCaseNamingStrategy
            );
    }
}
