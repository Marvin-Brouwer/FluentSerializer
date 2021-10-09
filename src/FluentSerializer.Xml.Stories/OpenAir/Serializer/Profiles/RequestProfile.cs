using FluentSerializer.Xml.Stories.OpenAir.Models.Request;
using FluentSerializer.Xml.Stories.OpenAir.Serializer.Converters;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
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
                .Child(response => response.ReadRequests,
                    namingStrategy: CustomNamingStrategy("Read"),
                    converter: NonWrappedListConverter)
                .Child(response => response.AddRequests,
                    namingStrategy: CustomNamingStrategy("Add"),
                    converter: NonWrappedListConverter)
                .Child(response => response.ModifyRequests,
                    namingStrategy: CustomNamingStrategy("Modify"),
                    converter: NonWrappedListConverter)
                .Child(response => response.DeleteRequests,
                    namingStrategy: CustomNamingStrategy("Delete"),
                    converter: NonWrappedListConverter);

            For<GetRequest<object>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: PascalCaseNamingStrategy
            )
                .Attribute(responseObject => responseObject.Filter)
                .Attribute(responseObject => responseObject.Type,
                    converter: RequestTypeValueConverter)
                .Child(response => response.Data,
                    converter: NonWrappedListConverter);

            For<RequestObject<object>>(
                attributeNamingStrategy: SnakeCaseNamingStrategy,
                tagNamingStrategy: PascalCaseNamingStrategy
            )
                .Attribute(responseObject => responseObject.Type,
                    converter: RequestTypeValueConverter)
                .Child(response => response.Data,
                    converter: NonWrappedListConverter);
        }
    }
}
