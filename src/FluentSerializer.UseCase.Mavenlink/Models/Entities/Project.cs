using System;

namespace FluentSerializer.UseCase.Mavenlink.Models;

internal sealed class Project : IMavenlinkEntity
{
	public string Id { get; set; } = string.Empty;
	public string? Name { get; set; }
	public string? ExternalId { get; set; }
	public DateTime LastUpdate { get; set; }
	public DateTime? CustomDate { get; set; }
}