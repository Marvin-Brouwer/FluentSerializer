﻿namespace FluentSerializer.Json.DataNodes
{
    public interface IJsonProperty : IJsonContainer<IJsonProperty>, IJsonObjectContent { 
        IJsonNode? Value { get; }
    }
}