namespace FluentSerializer.Core
{
    public interface ISerializer
    {
        public string Serialize<TData>(TData dataObject);
        public TData Deserialize<TData>(string stringData);
    }
}
