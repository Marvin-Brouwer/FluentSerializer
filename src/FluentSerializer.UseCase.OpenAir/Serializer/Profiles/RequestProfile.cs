using FluentSerializer.UseCase.OpenAir.Models.Request;
using FluentSerializer.UseCase.OpenAir.Serializer.Converters;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles
{
    public sealed class RequestProfile : OpenAirSerializerProfile
    {
        /// <inheritdoc cref="RequestTypeValueConverter.RequestTypeValueConverter"/>
        private static readonly RequestTypeValueConverter RequestTypeValueConverter = new RequestTypeValueConverter();

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
                .Attribute(responseObject => responseObject.Filter)
                .Attribute(responseObject => responseObject.Type,
                    converter: RequestTypeValueConverter)
                .Child(response => response.Data,
                    converter: NonWrappedCollectionConverter);

            For<AddRequest<object>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Add")
            )
                .Attribute(responseObject => responseObject.Type,
                    converter: RequestTypeValueConverter)
                .Child(response => response.Data,
                     converter: NonWrappedCollectionConverter);

            For<ModifyRequest<object>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Modify")
            )
                .Attribute(responseObject => responseObject.Type,
                    converter: RequestTypeValueConverter)
                .Child(response => response.Data,
                     converter: NonWrappedCollectionConverter);

            For<DeleteRequest<object>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: CustomNamingStrategy("Delete")
            )
                .Attribute(responseObject => responseObject.Type,
                    converter: RequestTypeValueConverter)
                .Child(response => response.Data,
                     converter: NonWrappedCollectionConverter);
        }
    }
}
