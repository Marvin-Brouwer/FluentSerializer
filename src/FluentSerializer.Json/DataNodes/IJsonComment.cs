using System.Diagnostics.CodeAnalysis;
using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Json.DataNodes
{
    [SuppressMessage("Minor Code Smell", "S1939:Inheritance list should not be redundant", Justification = "Clarity")]
    public interface IJsonComment : IDataValue, IJsonNode, IJsonObjectContent, IJsonArrayContent { }
}
