using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink
{
    public sealed partial class MavenlinkTests
    {
        private static readonly Request<Project> ProjectRequestExample = new()
        {
            Data = new()
            {
                new Project
                {
                    Id = "0001",
                    Name = "Project 1",
                    ExternalId = "P-00-1",
                    LastUpdate = CreateDate("1991-11-28 03:00:00"),
                    CustomDate = CreateDate("1991-11-28 03:00:00"),
                }
                
            }
        };
    }
}
