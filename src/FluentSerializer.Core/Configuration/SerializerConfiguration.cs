using System.Collections.Generic;
using System.Text;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Converting;

namespace FluentSerializer.Core.Configuration;

/// <summary>
/// The base serializer configuration necessary for FluentSerializers
/// </summary>
/// <inheritdoc cref="ITextConfiguration"/>
/// <inheritdoc cref="ISerializerConfiguration"/>
public abstract class SerializerConfiguration : ITextConfiguration, ISerializerConfiguration
{
	/// <inheritdoc />
	public int StringBuilderInitialCapacity { get; } = 100;
	/// <inheritdoc />
	public int StringBuilderMaximumRetainedCapacity { get; } = 4096;

	/// <inheritdoc />
	public bool FormatOutput { get; set; } = true;
	/// <inheritdoc />
	public bool WriteNull { get; set; } = false;

	/// <inheritdoc />
	public List<IConverter> DefaultConverters { get; set; } = new();
	/// <inheritdoc />
	public Encoding Encoding { get; set; } = Encoding.Default;
	/// <inheritdoc />
	public string NewLine { get; set; } = LineEndings.Environment;
}