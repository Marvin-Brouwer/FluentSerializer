using FluentSerializer.Core.Converting;
using System.Collections.Generic;

namespace FluentSerializer.Core.Configuration
{
	public interface ISerializerConfiguration
	{
		List<IConverter> DefaultConverters { get; set; }
		bool FormatOutput { get; set; }
		bool WriteNull { get; set; }
	}
}