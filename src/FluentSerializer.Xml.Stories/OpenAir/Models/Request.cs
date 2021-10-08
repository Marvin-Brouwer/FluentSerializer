using System.Collections.Generic;

namespace FluentSerializer.Xml.Stories.OpenAir.Models
{
    internal class Request<TRequest>
    {
        public List<GetRequest<TRequest>> GetRequest { get; set; }
        public List<RequestObject<TRequest>> AddRequest { get; set; }
        public List<RequestObject<TRequest>> ModifyRequest { get; set; }
        public List<RequestObject<TRequest>> DeleteRequest { get; set; }
    }

    internal class RequestObject<TRequest>
    {
        public List<TRequest> Data { get; set; }
    }

    internal class GetRequest<TRequest>
    {
        public string Filter { get; set; }
    }
}
