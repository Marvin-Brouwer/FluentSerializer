using System;
using System.Collections.Generic;
using System.Text;

namespace FluentSerializer.Json.Stories.Mavenlink.Models
{
    internal class Response<TResponse> where TResponse : IMavenlinkEntity
    {
        public int Count { get; set; }
        public List<TResponse> Data { get; set; }
    }
}
