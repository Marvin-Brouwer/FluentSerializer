using FluentSerializer.UseCase.OpenAir.Models.Request;
using FluentSerializer.UseCase.OpenAir.Serializer.Profiles.Base;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles
{
    public sealed class RequestProfile : OpenAirSerializerProfile
    {
        public override void Configure()
        {
            For<Request<object>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: LowerCaseNamingStrategy
            )
                .Child(response => response.Authentication)
                .Child(response => response.ReadRequests,
                    converter: NonWrappedCollectionConverter)
                .Child(response => response.AddRequests,
                    converter: NonWrappedCollectionConverter)
                .Child(response => response.ModifyRequests,
                    converter: NonWrappedCollectionConverter)
                .Child(response => response.DeleteRequests,
                    converter: NonWrappedCollectionConverter);

            For<ReadRequest<object>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Read")
            )
                .UseBase()
                .Attribute(responseObject => responseObject.Filter);

            For<AddRequest<object>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Add")
            )
                .UseBase();

            For<ModifyRequest<object>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Modify")
            )
                .UseBase();

            For<DeleteRequest<object>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Delete")
            )
                .UseBase();
        }
    }
}
