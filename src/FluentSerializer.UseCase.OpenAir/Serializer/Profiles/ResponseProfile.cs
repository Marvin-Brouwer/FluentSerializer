using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming;
using FluentSerializer.UseCase.OpenAir.Models.Base;
using FluentSerializer.UseCase.OpenAir.Models.Response;
using FluentSerializer.UseCase.OpenAir.Serializer.Profiles.Base;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Profiles;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles
{
    public sealed class ResponseProfile : XmlSerializerProfile
    {
        protected override void Configure()
        {
            For<Response<OpenAirEntity>>(
                direction: SerializerDirection.Deserialize,
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Use.LowerCase
            )
                .Child(response => response.ReadResponses,
                    namingStrategy: Names.Are("Read"),
                    converter: Converter.For.Collection(false))
                .Child(response => response.AddResponses,
                    namingStrategy: Names.Are("Add"),
                    converter: Converter.For.Collection(false))
                .Child(response => response.ModifyResponses,
                    namingStrategy: Names.Are("Modify"),
                    converter: Converter.For.Collection(false))
                .Child(response => response.DeleteResponses,
                    namingStrategy: Names.Are("Delete"),
                    converter: Converter.For.Collection(false));

            For<ReadResponse<OpenAirEntity>>(
                direction: SerializerDirection.Deserialize,
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Are("Read")
            )
                .UseBase();

            For<AddResponse<OpenAirEntity>>(
                direction: SerializerDirection.Deserialize,
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Are("Add")
            )
                .UseBase();

            For<ModifyResponse<OpenAirEntity>>(
                direction: SerializerDirection.Deserialize,
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Are("Modify")
            )
                .UseBase();

            For<DeleteResponse<OpenAirEntity>>(
                direction: SerializerDirection.Deserialize,
                attributeNamingStrategy: Names.Use.SnakeCase,
                tagNamingStrategy: Names.Are("Delete")
            )
                .UseBase();
        }
    }
}
