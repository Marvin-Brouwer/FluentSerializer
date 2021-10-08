using System.Collections.Generic;

namespace FluentSerializer.Json.Stories.OpenAir.Models
{
    internal class Response<TResponse> where TResponse : IOpenAirEntity
    {
        public List<ResponseObject<TResponse>> GetResponses { get; set; }
        public List<ResponseObject<TResponse>> AddResponses { get; set; }
        public List<ResponseObject<TResponse>> ModifyResponses { get; set; }
        public List<ResponseObject<TResponse>> DeleteResponses { get; set; }
    }

    internal class ResponseObject<TResponse> where TResponse : IOpenAirEntity
    {
        public int StatusCode { get; set; }
        public List<TResponse> Data { get; set; }
    }
}
