using FluentSerializer.Core.Services;
using System.Collections.Generic;

namespace FluentSerializer.Core.Configuration
{
    public sealed class SerializerConfiguration
    {
        public bool FormatOutput { get; set; }
        public List<IConverter> DefaultConverters { get; set; } = new List<IConverter>();
    }
}
