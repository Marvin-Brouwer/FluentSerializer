using FluentSerializer.UseCase.OpenAir.Models.Base;

using System.Collections.Generic;

namespace FluentSerializer.UseCase.OpenAir.Models.Response;

internal sealed class Response<TResponse> where TResponse : OpenAirEntity
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

internal sealed class ReadResponse<TResponse> : ResponseObject<TResponse>
	where TResponse : OpenAirEntity
{
}

internal sealed class AddResponse<TResponse> : ResponseObject<TResponse>
	where TResponse : OpenAirEntity
{

}

internal sealed class ModifyResponse<TResponse> : ResponseObject<TResponse>
	where TResponse : OpenAirEntity
{

}

internal sealed class DeleteResponse<TResponse> : ResponseObject<TResponse>
	where TResponse : OpenAirEntity
{

}