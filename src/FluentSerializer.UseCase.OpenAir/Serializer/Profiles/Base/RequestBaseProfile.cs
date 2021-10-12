using FluentSerializer.UseCase.OpenAir.Models.Request;
using FluentSerializer.UseCase.OpenAir.Serializer.Converters;
using FluentSerializer.Xml.Converters;
using FluentSerializer.Xml.Profiles;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles.Base
{
    /// <remarks>
    /// This is a temporary setup, in the final implementation the For{T} method will have an overload that supports base types.
    /// </remarks>
    public static class RequestBaseProfile
    {
        /// <inheritdoc cref="RequestTypeValueConverter"/>
        private static readonly RequestTypeValueConverter RequestTypeValueConverter = new RequestTypeValueConverter();
        private static readonly NonWrappedCollectionConverter NonWrappedCollectionConverter = new NonWrappedCollectionConverter();

        internal static IXmlProfileBuilder<TRequestObject> UseBase<TRequestObject>(this IXmlProfileBuilder<TRequestObject> builder)
            where TRequestObject : RequestObject<object>, new()
        {
            builder 
                .Attribute(responseObject => responseObject.Type,
                    converter: RequestTypeValueConverter)
                .Child(response => response.Data,
                    converter: NonWrappedCollectionConverter);

            return builder;
        }
    }
}
