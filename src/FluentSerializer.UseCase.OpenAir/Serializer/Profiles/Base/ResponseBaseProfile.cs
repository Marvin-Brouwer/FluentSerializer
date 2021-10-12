using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using FluentSerializer.UseCase.OpenAir.Models.Base;
using FluentSerializer.UseCase.OpenAir.Models.Response;
using FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies;
using FluentSerializer.Xml.Converters;
using FluentSerializer.Xml.Profiles;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles.Base
{
    /// <remarks>
    /// This is a temporary setup, in the final implementation the For{T} method will have an overload that supports base types.
    /// </remarks>
    public static class ResponseBaseProfile
    {
        /// <inheritdoc cref="NonWrappedCollectionConverter"/>
        private static readonly IConverter NonWrappedCollectionConverter = new NonWrappedCollectionConverter();
        /// <inheritdoc cref="ResponseTypeNamingStrategy"/>
        private static readonly INamingStrategy ResponseTypeNamingStrategy = new ResponseTypeNamingStrategy();
        /// <inheritdoc cref="CustomNamingStrategy"/>
        private static INamingStrategy CustomNamingStrategy(string name) => new CustomNamingStrategy(name);

        internal static IXmlProfileBuilder<TResponseObject> UseBase<TResponseObject>(this IXmlProfileBuilder<TResponseObject> builder)
            where TResponseObject : ResponseObject<OpenAirEntity>, new()
        {
            builder 
                .Attribute(responseObject => responseObject.StatusCode,
                    namingStrategy: CustomNamingStrategy("status"))
                .Child(response => response.Data,
                    namingStrategy: ResponseTypeNamingStrategy,
                    converter: NonWrappedCollectionConverter);

            return builder;
        }

    }
}
