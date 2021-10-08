using System.Collections.Generic;

namespace FluentSerializer.Json.Stories.Mavenlink.Models
{
    internal class Request<TRequest>
    {
        public List<TRequest> Data { get; set; }
    }
}
