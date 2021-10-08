using FluentSerializer.Xml.Tests;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public sealed class ProjectProfile : OpenAirSerializerProfile
    {
        public override void Configure()
        {
            For<Project>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: PascalCaseNamingStrategy
            )
                .Child(project => project.Name)
                .Child(project => project.LastUpdate,
                    namingStrategy: CustomNamingStrategy("updated"),
                    converter: OpenAirDateConverter)
                .Child(project => project.Active,
                    converter: StringBitBooleanConverter)
                .Child(project => project.CustomDate,
                    namingStrategy: CustomFieldNamingStrategy,
                    converter: SimpleDateConverter)
                .Child(project => project.RateCardId);
        }
    }
}
