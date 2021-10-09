namespace FluentSerializer.Core
{
    public interface ISerializer
    {
        public string Serialize<TModel>(TModel model);
        public TModel? Deserialize<TModel>(string stringData) where TModel : class, new();
    }
}
