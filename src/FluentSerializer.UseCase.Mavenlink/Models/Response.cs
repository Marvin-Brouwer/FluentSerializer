using FluentSerializer.UseCase.Mavenlink.Models.Entities;

using System.Collections.Generic;

namespace FluentSerializer.UseCase.Mavenlink.Models;

internal sealed class Response<TResponse> where TResponse : IMavenlinkEntity
{
	public int Count { get; set; } = default!;
	public int PageCount { get; set; } = default!;
	public int CurrentPage { get; set; } = default!;

	public List<TResponse> Data { get; set; } = new();
}