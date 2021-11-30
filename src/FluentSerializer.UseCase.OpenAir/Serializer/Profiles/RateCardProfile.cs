using FluentSerializer.Core.Naming;
using FluentSerializer.UseCase.OpenAir.Models;
using FluentSerializer.UseCase.OpenAir.Serializer.Converters;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Profiles;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles
{
    public sealed class RateCardProfile : XmlSerializerProfile
    {
        protected override void Configure()
        {
            For<RateCard>(
                tagNamingStrategy: Names.Are("Ratecard")
            )
                .Child(rateCard => rateCard.Id)
                .Child(rateCard => rateCard.Name)
                .Child(project => project.LastUpdate,
                    namingStrategy: Names.Are("updated"),
                    converter: Converter.For.OpenAirDate);
        }
    }
}
