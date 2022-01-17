using System.Collections.Generic;
using System.Text;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Converting;

namespace FluentSerializer.Core.Configuration;

public abstract class SerializerConfiguration : ITextConfiguration, ISerializerConfiguration
{
	public int StringBuilderInitialCapacity { get; } = 100;
	public int StringBuilderMaximumRetainedCapacity { get; } = 4096;

	public bool FormatOutput { get; set; } = true;
	public bool WriteNull { get; set; } = false;
	public bool UseWriteArrayPool { get; set; } = true;

	public List<IConverter> DefaultConverters { get; set; } = new();
	public Encoding Encoding { get; set; } = Encoding.Default;
	public string NewLine { get; set; } = LineEndings.Environment;
}