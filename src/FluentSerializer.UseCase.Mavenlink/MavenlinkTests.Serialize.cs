using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;

namespace FluentSerializer.UseCase.Mavenlink;

public sealed partial class MavenlinkTests
{
	private static readonly Request<Project> ProjectRequestExample = new()
	{
		Data = new Project
		{
			Id = "0001",
			Name = "Project 1",
			ExternalId = "P-00-1",
			LastUpdate = CreateDate("1991-11-28 04:00:00"),
			CustomDate = CreateDate("1991-11-28 03:00:00"),
		}
	};
}