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
        private static readonly Response<User> UserResponseExample = new Response<User>
        {
            Count = 1,
            Data = new List<User>
            {
                new User
                {
                    Id = "U1"
                }
            }
        };
    }
}
