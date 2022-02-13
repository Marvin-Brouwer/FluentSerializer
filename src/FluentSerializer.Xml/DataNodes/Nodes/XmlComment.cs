using System.Diagnostics;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlComment"/>
[DebuggerDisplay("<!-- {Value, nq} -->")]
public readonly partial struct XmlComment : IXmlComment
{
	private const string CommentName = "<!-- comment -->";

	/// <inheritdoc />
	public string Name => CommentName;
	/// <inheritdoc />
	public string? Value { get; }

	/// <inheritdoc cref="XmlBuilder.Comment"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Comment"/> method instead of this constructor</b>
	/// </remarks>
	public XmlComment(in string? value = null)
	{
		Value = value;
	}
}