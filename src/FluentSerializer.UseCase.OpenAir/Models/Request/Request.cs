using System.Collections.Generic;
using System.Xml.Linq;

namespace FluentSerializer.UseCase.OpenAir.Models.Request
{
    internal class Request<TRequest>
    {
        public XComment Authentication => new(" Normally this is where the authentication element would be added ");

        public List<ReadRequest<TRequest>>? ReadRequests { get; set; } 
        public List<AddRequest<TRequest>>? AddRequests { get; set; }
        public List<ModifyRequest<TRequest>>? ModifyRequests { get; set; }
        public List<DeleteRequest<TRequest>>? DeleteRequests { get; set; }
    }

    internal class RequestObject<TRequest>
    {
        public List<TRequest> Data { get; set; } = new List<TRequest>();
        public string Type { get; set; } = string.Empty;
    }

    internal class ReadRequest<TRequest> : RequestObject<TRequest>
    {
        public string Filter { get; set; } = string.Empty;
    }
    internal class AddRequest<TRequest> : RequestObject<TRequest>
    {
    }
    internal class ModifyRequest<TRequest> : RequestObject<TRequest>
    {
    }
    internal class DeleteRequest<TRequest> : RequestObject<TRequest>
    {
    }
}
