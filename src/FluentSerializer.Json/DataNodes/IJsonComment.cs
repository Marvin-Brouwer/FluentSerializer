using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Json.DataNodes;

/// <summary>
/// A representation of a JSON comment <br/>
/// This is not in spec but supported eitherway. <br/><br/>
/// <code>// comment</code>
/// <code>/* comment */</code>
/// </summary>
public interface IJsonComment : IDataValue, IJsonNode, IJsonObjectContent, IJsonArrayContent { }