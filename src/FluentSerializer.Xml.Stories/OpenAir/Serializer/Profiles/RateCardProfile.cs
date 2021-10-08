using FluentSerializer.Xml.Tests;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public sealed class RateCardProfile : OpenAirSerializerProfile
    {
        public override void Configure()
        {
            For<RateCard>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Ratecard")
            )
                .Child(rateCard => rateCard.Name);
        }
    }
}
