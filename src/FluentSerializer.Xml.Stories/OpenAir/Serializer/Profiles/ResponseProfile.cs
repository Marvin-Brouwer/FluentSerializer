using FluentSerializer.Xml.Stories.OpenAir.Models;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public sealed class ResponseProfile : OpenAirSerializerProfile
    {
        public override void Configure()
        {
            For<Response<IOpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: LowerCaseNamingStrategy
            )
                .Child(response => response.ReadResponses,
                    namingStrategy: CustomNamingStrategy("Read"),
                    converter: NonWrappedListConverter)
                .Child(response => response.AddResponses,
                    namingStrategy: CustomNamingStrategy("Add"),
                    converter: NonWrappedListConverter)
                .Child(response => response.ModifyResponses,
                    namingStrategy: CustomNamingStrategy("Modify"),
                    converter: NonWrappedListConverter)
                .Child(response => response.DeleteResponses,
                    namingStrategy: CustomNamingStrategy("Delete"),
                    converter: NonWrappedListConverter);

            For<ResponseObject<IOpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: PascalCaseNamingStrategy
            )
                .Attribute(responseObject => responseObject.StatusCode,
                    namingStrategy: CustomNamingStrategy("status"))
                .Child(response => response.Data,
                    converter: NonWrappedListConverter);
        }
    }
}
