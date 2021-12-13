using System.IO;
using System.Text;
using Ardalis.GuardClauses;

namespace FluentSerializer.Core.Services;

public sealed class ConfigurableStringWriter : StringWriter
{
	public override Encoding Encoding { get; }

	public ConfigurableStringWriter(StringBuilder stringBuilder, Encoding encoding) : base(stringBuilder)
	{
		Guard.Against.Null(stringBuilder, nameof(stringBuilder));
		Guard.Against.Null(encoding, nameof(encoding));

		Encoding = encoding;
	}
}