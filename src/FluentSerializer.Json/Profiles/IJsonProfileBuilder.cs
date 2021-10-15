using System;
using System.Linq.Expressions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Json.Converting;

namespace FluentSerializer.Json.Profiles
{
    public interface IJsonProfileBuilder<TModel>
        where TModel : new()
    {

        public IJsonProfileBuilder<TModel> Property<TProperty>(
            Expression<Func<TModel, TProperty>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            Func<INamingStrategy>? namingStrategy = null,
            Func<IJsonConverter>? converter = null
        );
    }
}