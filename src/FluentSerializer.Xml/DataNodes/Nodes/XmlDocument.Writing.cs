using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.Configuration;

using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlDocument
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0)
	{
		var stringBuilder = stringBuilders.Get();

		try
		{
			stringBuilder
				.Append($"<?xml version=\"1.0\" encoding=\"{stringBuilder.TextConfiguration.Encoding.WebName}\"?>")
				.AppendOptionalNewline(in format);

			AppendTo(ref stringBuilder, in format, in indent, in writeNull);
			return stringBuilder.ToString();
		}
		finally
		{
			stringBuilders.Return(stringBuilder);
		}
	}

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		return RootElement?.AppendTo(ref stringBuilder, in format, in indent, in writeNull) ?? stringBuilder;
	}
}