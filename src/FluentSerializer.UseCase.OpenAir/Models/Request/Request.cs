using FluentSerializer.Xml.DataNodes;
using System.Collections.Generic;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.UseCase.OpenAir.Models.Request;

internal class Request<TRequest>
{
#pragma warning disable CA1822 // Mark members as static
	public IXmlComment Authentication => Comment("Normally this is where the authentication element would be added");
#pragma warning restore CA1822 // Mark members as static

	public List<ReadRequest<TRequest>>? ReadRequests { get; set; } 
	public List<AddRequest<TRequest>>? AddRequests { get; set; }
	public List<ModifyRequest<TRequest>>? ModifyRequests { get; set; }
	public List<DeleteRequest<TRequest>>? DeleteRequests { get; set; }
}

internal class RequestObject<TRequest>
{
	public List<TRequest> Data { get; set; } = new();
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