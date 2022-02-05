using FluentSerializer.Core.Converting;
using System.Collections.Generic;

namespace FluentSerializer.Core.Configuration;

/// <summary>
/// Configuration properties shared by every FluentSerializer
/// </summary>
public interface ISerializerConfiguration
{
	/// <summary>
	/// The converters used by the serializer when no property specific converter is applied. <br />
	/// <example>
	/// Use Converter.For, Eg. <![CDATA[configuration.DefaultConverters.Add(Converter.For.Json());]]>
	/// </example>
	/// </summary>
	List<IConverter> DefaultConverters { get; set; }

	/// <summary>
	/// Determine whether to format the serialized data or not.
	/// </summary>
	/// <remarks>
	/// This may end up not being useful, if no usecases are available this property may be removed in a later version.
	/// </remarks>
	bool FormatOutput { get; set; }

	/// <summary>
	/// Determine wheter to show null values in serialized output
	/// </summary>
	bool WriteNull { get; set; }
}