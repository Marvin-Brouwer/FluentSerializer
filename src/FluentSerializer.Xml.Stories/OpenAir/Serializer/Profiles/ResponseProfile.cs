using FluentSerializer.Xml.Stories.OpenAir.Models;
using FluentSerializer.Xml.Stories.OpenAir.Models.Response;

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
                    converter: NonWrappedCollectionConverter)
                .Child(response => response.AddResponses,
                    namingStrategy: CustomNamingStrategy("Add"),
                    converter: NonWrappedCollectionConverter)
                .Child(response => response.ModifyResponses,
                    namingStrategy: CustomNamingStrategy("Modify"),
                    converter: NonWrappedCollectionConverter)
                .Child(response => response.DeleteResponses,
                    namingStrategy: CustomNamingStrategy("Delete"),
                    converter: NonWrappedCollectionConverter);

            For<ReadResponse<IOpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Read")
            )
                .Attribute(responseObject => responseObject.StatusCode,
                    namingStrategy: CustomNamingStrategy("status"))
                .Child(response => response.Data,
                    //namingStrategy: ResponseTypeNamingStrategy,
                    converter: NonWrappedCollectionConverter);

            For<AddResponse<IOpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Add")
            )
                .Attribute(responseObject => responseObject.StatusCode,
                    namingStrategy: CustomNamingStrategy("status"))
                .Child(response => response.Data,
                    converter: NonWrappedCollectionConverter);

            For<ModifyResponse<IOpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Modify")
            )
                .Attribute(responseObject => responseObject.StatusCode,
                    namingStrategy: CustomNamingStrategy("status"))
                .Child(response => response.Data,
                    converter: NonWrappedCollectionConverter);

            For<DeleteResponse<IOpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Delete")
            )
                .Attribute(responseObject => responseObject.StatusCode,
                    namingStrategy: CustomNamingStrategy("status"))
                .Child(response => response.Data,
                    converter: NonWrappedCollectionConverter);
        }
    }
}
