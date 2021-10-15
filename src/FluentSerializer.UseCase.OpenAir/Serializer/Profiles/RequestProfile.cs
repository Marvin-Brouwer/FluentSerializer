﻿using FluentSerializer.Core.Naming;
using FluentSerializer.UseCase.OpenAir.Models.Request;
using FluentSerializer.UseCase.OpenAir.Serializer.Profiles.Base;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Profiles;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles
{
    public sealed class RequestProfile : XmlSerializerProfile
    {
        public override void Configure()
        {
            For<Request<object>>(
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Use.LowerCase
            )
                .Child(response => response.Authentication)
                .Child(response => response.ReadRequests,
                    converter: Converter.For.Collection(false))
                .Child(response => response.AddRequests,
                    converter: Converter.For.Collection(false))
                .Child(response => response.ModifyRequests,
                    converter: Converter.For.Collection(false))
                .Child(response => response.DeleteRequests,
                    converter: Converter.For.Collection(false));

            For<ReadRequest<object>>(
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Are("Read")
            )
                .UseBase()
                .Attribute(responseObject => responseObject.Filter);

            For<AddRequest<object>>(
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Are("Add")
            )
                .UseBase();

            For<ModifyRequest<object>>(
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Are("Modify")
            )
                .UseBase();

            For<DeleteRequest<object>>(
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Are("Delete")
            )
                .UseBase();
        }
    }
}