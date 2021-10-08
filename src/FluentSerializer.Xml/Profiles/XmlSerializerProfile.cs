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
        protected readonly PascalCaseNamingStrategy PascalCaseNamingStrategy = new PascalCaseNamingStrategy();
        protected readonly SnakeCaseNamingStrategy SnakeCaseNamingStrategy = new SnakeCaseNamingStrategy();

        protected DateByFormatConverter DateByFormatConverter(
            string format, CultureInfo? cultureInfo = null, DateTimeStyles? dateTimeStyle = null) => 
            new DateByFormatConverter(format, cultureInfo ?? CultureInfo.CurrentCulture, dateTimeStyle ?? DateTimeStyles.None);

        protected XmlProfileBuilder<TModel> For<TModel>(
            INamingStrategy? rootNamingStrategy = null,
            INamingStrategy? defaultNamingStrategy = null)
            where TModel : new() =>
            new XmlProfileBuilder<TModel>(
                rootNamingStrategy ?? PascalCaseNamingStrategy,
                defaultNamingStrategy ?? PascalCaseNamingStrategy
            );
    }
}
