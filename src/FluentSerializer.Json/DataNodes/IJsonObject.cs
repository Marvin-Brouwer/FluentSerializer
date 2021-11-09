namespace FluentSerializer.Json.DataNodes
{
    public interface IJsonObject : IJsonContainer<IJsonObject>, IJsonArrayContent, IJsonPropertyContent
    {
        IJsonProperty? GetProperty(string name);
    }
}