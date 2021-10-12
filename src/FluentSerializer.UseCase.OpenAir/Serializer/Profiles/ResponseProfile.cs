using FluentSerializer.UseCase.OpenAir.Models.Base;
using FluentSerializer.UseCase.OpenAir.Models.Response;
using FluentSerializer.UseCase.OpenAir.Serializer.Profiles.Base;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles
{
    public sealed class ResponseProfile : OpenAirSerializerProfile
    {
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
                .UseBase();

            For<AddResponse<OpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Add")
            )
                .UseBase();

            For<ModifyResponse<OpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Modify")
            )
                .UseBase();

            For<DeleteResponse<OpenAirEntity>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Delete")
            )
                .UseBase();
        }
    }
}
