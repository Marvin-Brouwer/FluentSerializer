using System.Collections.Generic;
using System.Text;
using FluentSerializer.Core.Converting;

namespace FluentSerializer.Core.Configuration
{
    public sealed class SerializerConfiguration
    {
        public bool FormatOutput { get; set; }
        public List<IConverter> DefaultConverters { get; set; } = new List<IConverter>();
        public Encoding Encoding { get; set; } = Encoding.Unicode;
    }
}
