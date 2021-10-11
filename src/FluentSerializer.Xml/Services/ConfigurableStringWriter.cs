using Ardalis.GuardClauses;
using System.IO;
using System.Text;

namespace FluentSerializer.Xml.Services
{
    public sealed class ConfigurableStringWriter : StringWriter
    {
        private readonly Encoding _encoding;
        public override Encoding Encoding => _encoding;

        public ConfigurableStringWriter(StringBuilder stringBuilder, Encoding encoding) : base(stringBuilder)
        {
            Guard.Against.Null(stringBuilder, nameof(stringBuilder));
            Guard.Against.Null(encoding, nameof(encoding));

            _encoding = encoding;
        }
    }
}
