namespace FluentSerializer.Core.Data
{
    public interface IDataValue : IDataNode
    {
        string? Value { get; }
    }
}
