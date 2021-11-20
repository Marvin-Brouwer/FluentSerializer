using System.Collections.Generic;

namespace FluentSerializer.UseCase.Mavenlink.Models
{
    internal class Response<TResponse> where TResponse : IMavenlinkEntity
    {
        public int Count { get; set; }
        public List<TResponse> Data { get; set; } = new ();
    }
}
