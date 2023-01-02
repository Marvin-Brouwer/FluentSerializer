using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Json.Converting;

using System;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Converters;

public static class ConverterExtensions
{
	private static readonly MavenlinkResponseMetaDataConverter MavenlinkResponsePageCountConverter = new("page_count");
	private static readonly MavenlinkResponseMetaDataConverter MavenlinkResponseCurrentPageConverter = new("page_number");
	private static readonly MavenlinkResponseDataConverter MavenlinkResponseDataConverter = new();
	private static readonly MavenlinkIdReferenceConverter MavenlinkIdReferenceConverter = new();
	private static readonly MavenlinkIdListReferenceConverter MavenlinkIdListReferenceConverter = new();

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

	/// <inheritdoc cref="MavenlinkIdReferenceConverter"/>
	public static IJsonConverter Reference(this IUseJsonConverters _) => MavenlinkIdReferenceConverter;

	/// <inheritdoc cref="MavenlinkIdListReferenceConverter"/>
	public static IJsonConverter References(this IUseJsonConverters _) => MavenlinkIdListReferenceConverter;

	/// <inheritdoc cref="MavenlinkCustomFieldReferenceConverter"/>
	public static Func<IJsonConverter> CustomFieldReference(this IUseJsonConverters _, Func<INamingStrategy> namingStrategy) => () => new MavenlinkCustomFieldReferenceConverter(namingStrategy);

}
