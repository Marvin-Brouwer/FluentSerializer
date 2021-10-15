using System.Collections.Generic;
using FluentSerializer.UseCase.OpenAir.Models;
using FluentSerializer.UseCase.OpenAir.Models.Request;

namespace FluentSerializer.UseCase.OpenAir
{
    public sealed partial class OpenAirTests
    {
        private static readonly Request<Project> ProjectRequestExample = new Request<Project>
        {
            AddRequests = new List<AddRequest<Project>>
            {
                new AddRequest<Project>
                {
                    Data = new List<Project>
                    {
                        new Project
                        {
                            Id = "0001",
                            Active = true,
                            Name = "Project 1",
                            ExternalId = "P-00-1",
                            LastUpdate = CreateDate("1991-11-28 03:00:00"),
                            CustomDate = CreateDate("1991-11-28 03:00:00"),
                        }
                    }
                },
                new AddRequest<Project>{
                    Data = new List<Project>
                    {
                        new Project
                        {
                            Id = "0002",
                            Active = false,
                            Name = "Project 2",
                            LastUpdate = CreateDate("1991-11-28 04:00:00")
                        },
                        new Project
                        {
                            Id = "0003",
                            Active = true,
                            Name = "Project 3",
                            LastUpdate = CreateDate("1991-11-28 05:00:00")
                        }
                    }
                }
            }
        };
    }
}
