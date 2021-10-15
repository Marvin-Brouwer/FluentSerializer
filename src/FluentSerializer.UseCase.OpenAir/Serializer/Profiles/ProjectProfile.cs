﻿using FluentSerializer.Core.Naming;
using FluentSerializer.UseCase.OpenAir.Models;
using FluentSerializer.UseCase.OpenAir.Serializer.Converters;
using FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Profiles;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles
{
    public sealed class ProjectProfile : XmlSerializerProfile
    {
        public override void Configure()
        {
            For<Project>(
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Use.PascalCase
            )
                .Child(project => project.Id)
                .Child(project => project.Name)
                .Child(project => project.LastUpdate,
                    namingStrategy: Names.Are("updated"),
                    converter: Converter.For.OpenAirDate)
                .Child(project => project.Active,
                    converter: Converter.For.StringBitBoolean)
                .Child(project => project.CustomDate,
                    namingStrategy: Names.Use.CustomFieldNames,
                    converter: Converter.For.SimpleDate)
                .Child(project => project.ExternalId,
                    namingStrategy: Names.Use.CustomFieldName("some_external_service_name_id"))
                .Child(project => project.RateCardId);
        }
    }
}