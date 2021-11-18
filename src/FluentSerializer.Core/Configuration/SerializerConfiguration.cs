using System.Collections.Generic;
using System.Text;
using FluentSerializer.Core.Converting;

namespace FluentSerializer.Core.Configuration
{
    public abstract class SerializerConfiguration
    {
        public bool FormatOutput { get; set; } = true;
        public bool WriteNull { get; set; } = false;
        public List<IConverter> DefaultConverters { get; set; } = new();
        public Encoding Encoding { get; set; } = Encoding.Default;
    }
}
