using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.UseCase.OpenAir.Models;
using FluentSerializer.UseCase.OpenAir.Models.Response;
using FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles
{
    public sealed class ResponseProfile : OpenAirSerializerProfile
    {
        private readonly INamingStrategy ResponseTypeNamingStrategy = new ResponseTypeNamingStrategy();

        public override void Configure()
        {
            For<Response<OpenAirEntity>>(
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

            For<ReadResponse<OpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Read")
            )
                .Attribute(responseObject => responseObject.StatusCode,
                    namingStrategy: CustomNamingStrategy("status"))
                .Child(response => response.Data,
                    namingStrategy: ResponseTypeNamingStrategy,
                    converter: NonWrappedCollectionConverter);

            For<AddResponse<OpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Add")
            )
                .Attribute(responseObject => responseObject.StatusCode,
                    namingStrategy: CustomNamingStrategy("status"))
                .Child(response => response.Data,
                    converter: NonWrappedCollectionConverter);

            For<ModifyResponse<OpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Modify")
            )
                .Attribute(responseObject => responseObject.StatusCode,
                    namingStrategy: CustomNamingStrategy("status"))
                .Child(response => response.Data,
                    converter: NonWrappedCollectionConverter);

            For<DeleteResponse<OpenAirEntity>>(
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
