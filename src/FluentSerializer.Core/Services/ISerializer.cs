﻿using FluentSerializer.Core.Configuration;

namespace FluentSerializer.Core.Services
{
    public interface ISerializer
    {
        SerializerConfiguration Configuration { get; }

        public string Serialize<TModel>(TModel model);
        public TModel? Deserialize<TModel>(string stringData) where TModel : class, new();
    }
}
