using FluentSerializer.Xml.Tests;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    internal sealed class RateCardProfile : OpenAirSerializerProfile
    {
        public override void Configure()
        {
            For<RateCard>(
                defaultNamingStrategy: SnakeCaseNamingStrategy,
                rootNamingStrategy: CustomNamingStrategy("Ratecard")
            )
                .Child(rateCard => rateCard.Name);
        }
    }
}
