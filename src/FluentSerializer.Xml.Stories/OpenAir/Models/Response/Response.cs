using System.Collections.Generic;

namespace FluentSerializer.Xml.Stories.OpenAir.Models.Response
{
    internal class Response<TResponse> where TResponse : IOpenAirEntity
    {
        public List<ReadResponse<TResponse>> ReadResponses { get; set; } = new List<ReadResponse<TResponse>>();
        public List<AddResponse<TResponse>> AddResponses { get; set; } = new List<AddResponse<TResponse>>();
        public List<ModifyResponse<TResponse>> ModifyResponses { get; set; } = new List<ModifyResponse<TResponse>>();
        public List<DeleteResponse<TResponse>> DeleteResponses { get; set; } = new List<DeleteResponse<TResponse>>();
    }

    internal abstract class ResponseObject<TResponse>
        where TResponse : IOpenAirEntity
    {
        public int StatusCode { get; set; }
        public List<TResponse> Data { get; set; } = new List<TResponse>();
    }

    internal class ReadResponse<TResponse> : ResponseObject<TResponse>
        where TResponse : IOpenAirEntity
    {
    }

    internal class AddResponse<TResponse> : ResponseObject<TResponse>
        where TResponse : IOpenAirEntity
    {

    }

    internal class ModifyResponse<TResponse> : ResponseObject<TResponse>
        where TResponse : IOpenAirEntity
    {

    }

    internal class DeleteResponse<TResponse> : ResponseObject<TResponse>
        where TResponse : IOpenAirEntity
    {

    }
}
