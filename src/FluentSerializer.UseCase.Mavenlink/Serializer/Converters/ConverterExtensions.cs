using System;
using System.Linq.Expressions;
using FluentSerializer.Json.Converting;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Converters
{
    public static class ConverterExtensions
	{
		private static readonly MavenlinkResponseMetaDataConverter MavenlinkResponsePageCountConverter = new("page_count");
		private static readonly MavenlinkResponseMetaDataConverter MavenlinkResponseCurrentPageConverter = new("page_number");
		private static readonly MavenlinkResponseDataConverter MavenlinkResponseDataConverter = new();

		/// <summary>
		/// Pull the page count out of the metadata object
		/// </summary>
		public static IJsonConverter MavenlinkResponsePageCount(this IUseJsonConverters _) => MavenlinkResponsePageCountConverter;
		/// <summary>
		/// Pull the current page number out of the metadata object
		/// </summary>
		public static IJsonConverter MavenlinkResponseCurrentPage(this IUseJsonConverters _) => MavenlinkResponseCurrentPageConverter;

		/// <inheritdoc cref="Converters.MavenlinkResponseDataConverter"/>
		public static IJsonConverter MavenlinkResponseData(this IUseJsonConverters _) => MavenlinkResponseDataConverter;

		/// <inheritdoc cref="MavenlinkReferenceConverter{TClass}"/>
		public static Func<IReferenceConverter<TClass>> ReferenceTo<TClass>(this IUseJsonConverters _,
			Expression<Func<TClass, object?>> referenceField)
			where TClass : notnull => () => new MavenlinkReferenceConverter<TClass>(referenceField);

	}
}
