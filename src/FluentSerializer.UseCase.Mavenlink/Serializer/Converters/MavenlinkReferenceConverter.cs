using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;
using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Converters
{
	/// <summary>
	/// A custom converter since Mavenlink returns a list of id's with a reference to a collection instead of the nested object.
	/// </summary>
	/// <remarks>
	/// Because the <see cref="System.Collections.Generic.List{T}" /> will always contain one type we can just pick the first item in the list 
	/// to determine the collection to resolve.
	/// </remarks>
	/// <example>
	/// <![CDATA[
	/// {
	///     "results": [
	///         {
	///             "key": "users",
	///             "id": "9770395"
	///         },
	///         {
	///             "key": "users",
	///             "id": "9770335"
	///         }
	///     },
	///     "users": {
	///         "9770395": {
	///             ....
	///         },
	///         "9770335": {
	///             ....
	///         }
	///     ]
	/// }
	/// ]]>
	/// TODO
	/// </example>
	internal sealed class MavenlinkReferenceConverter<TClass> : IReferenceConverter<TClass>
	{
		private readonly PropertyInfo _referenceField;

		/// <inheritdoc />
		public SerializerDirection Direction => SerializerDirection.Deserialize;
		/// <inheritdoc />
		public bool CanConvert(in Type targetType) => _referenceField.PropertyType.IsAssignableFrom(targetType);

		/// <inheritdoc cref="MavenlinkResponseMetaDataConverter"/>
		public MavenlinkReferenceConverter(in Expression<Func<TClass, object?>> referenceField)
		{
			_referenceField = referenceField.GetProperty();
		}

		public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context) =>
			throw new NotSupportedException();

		public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
		{
			if (objectToDeserialize is not IJsonValue jsonValueToDeserialize) throw new NotSupportedException();
			
			var stringValue = jsonValueToDeserialize.Value;
			if (stringValue is null) return default;
			
			var targetNodeName = EntityMappings.GetDataItemName(_referenceField.PropertyType.Name);
			var targetNodeCollectionName = EntityMappings.GetDataGroupName(_referenceField.PropertyType.Name);
			var targetNodeReferenceCollection = (context.ParentNode as IJsonObject)!
				.GetProperty(targetNodeCollectionName)?
				.Value as IJsonArray;

			if (targetNodeReferenceCollection is null) return default;
			var targetReferenceNode = targetNodeReferenceCollection.Children
				.FirstOrDefault(child => child.Name.Equals(targetNodeName, StringComparison.Ordinal)) as IJsonObject;
			
			if (targetReferenceNode is null) return default;

			return ((IAdvancedJsonSerializer)context.CurrentSerializer)
				.Deserialize(targetReferenceNode, context.PropertyType);
		}
	}

	public interface IReferenceConverter<TClass> : IJsonConverter { }
}
