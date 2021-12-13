namespace FluentSerializer.Core.DataNodes;

public interface IDataValue : IDataNode
{
	string? Value { get; }
}