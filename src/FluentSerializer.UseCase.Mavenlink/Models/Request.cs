using System.Collections.Generic;

namespace FluentSerializer.UseCase.Mavenlink.Models
{
    internal class Request<TRequest>
    {
        public List<TRequest> Data { get; set; } = new ();
    }
}
