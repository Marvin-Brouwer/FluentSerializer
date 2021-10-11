﻿using FluentSerializer.Xml.Stories.OpenAir.Models;

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
                .Child(rateCard => rateCard.Id)
                .Child(rateCard => rateCard.Name)
                .Child(project => project.LastUpdate,
                    namingStrategy: CustomNamingStrategy("updated"),
                    converter: OpenAirDateConverter);
        }
    }
}