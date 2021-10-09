using System.Collections.Generic;

namespace FluentSerializer.Xml.Stories.OpenAir.Models
{
    internal class Request<TRequest>
    {
        public List<GetRequest<TRequest>> ReadRequests { get; set; } = new List<GetRequest<TRequest>>();
        public List<RequestObject<TRequest>> AddRequests { get; set; } = new List<RequestObject<TRequest>>();
        public List<RequestObject<TRequest>> ModifyRequests { get; set; } = new List<RequestObject<TRequest>>();
        public List<RequestObject<TRequest>> DeleteRequests { get; set; } = new List<RequestObject<TRequest>>();
    }

    internal class RequestObject<TRequest>
    {
        public List<TRequest> Data { get; set; } = new List<TRequest>();
        public string Type { get; set; } = string.Empty;
    }

    internal class GetRequest<TRequest> : RequestObject<TRequest>
    {
        public string Filter { get; set; } = string.Empty;
    }
}
