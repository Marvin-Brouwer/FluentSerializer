using System;
using System.Collections.Generic;
using System.Text;

namespace FluentSerializer.Json.Stories.Mavenlink.Models
{
    internal class Request<TRequest>
    {
        public List<TRequest> Data { get; set; }
    }
}
