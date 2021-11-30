using System.Collections.Generic;
using FluentSerializer.UseCase.OpenAir.Models.Base;

namespace FluentSerializer.UseCase.OpenAir.Models.Response
{
    internal class Response<TResponse> where TResponse : OpenAirEntity
    {
        public List<ReadResponse<TResponse>> ReadResponses { get; set; } = new();
        public List<AddResponse<TResponse>> AddResponses { get; set; } = new();
        public List<ModifyResponse<TResponse>> ModifyResponses { get; set; } = new();
        public List<DeleteResponse<TResponse>> DeleteResponses { get; set; } = new();
    }

    internal abstract class ResponseObject<TResponse>
        where TResponse : OpenAirEntity
    {
        public int StatusCode { get; set; }
        public List<TResponse> Data { get; set; } = new();
    }

    internal class ReadResponse<TResponse> : ResponseObject<TResponse>
        where TResponse : OpenAirEntity
    {
    }

    internal class AddResponse<TResponse> : ResponseObject<TResponse>
        where TResponse : OpenAirEntity
    {

    }

    internal class ModifyResponse<TResponse> : ResponseObject<TResponse>
        where TResponse : OpenAirEntity
    {

    }

    internal class DeleteResponse<TResponse> : ResponseObject<TResponse>
        where TResponse : OpenAirEntity
    {

    }
}
