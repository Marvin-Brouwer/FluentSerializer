using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Profiles;
using FluentSerializer.Json.Services;
using FluentSerializer.UseCase.Mavenlink.Models;
using Xunit;

namespace FluentSerializer.UseCase.Mavenlink
{
    public sealed partial class MavenlinkTests
    {
        private static readonly Request<Project> ProjectRequestExample = new Request<Project>
        {
            Data = new List<Project>
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
