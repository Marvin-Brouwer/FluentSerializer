using FluentSerializer.Core.Comparing;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Converting;

using System.Collections;
using System.Text;

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
	public ReferenceLoopBehavior ReferenceLoopBehavior { get; set; } = ReferenceLoopBehavior.Throw;
	/// <inheritdoc />
	public IEqualityComparer ReferenceComparer { get; set; } = DefaultReferenceComparer.Default;


	/// <inheritdoc />
	public bool FormatOutput { get; set; } = true;
	/// <inheritdoc />
	public bool WriteNull { get; set; }

	/// <inheritdoc />
	public IConfigurationStack<IConverter> DefaultConverters { get; set; } = new ConfigurationStack<IConverter>(
		ConverterComparer.Default
	);
	/// <inheritdoc />
	public Encoding Encoding { get; set; } = Encoding.Default;
	/// <inheritdoc />
	public string NewLine { get; set; } = LineEndings.Environment;}