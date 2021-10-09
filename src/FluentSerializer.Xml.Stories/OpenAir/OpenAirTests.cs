using FluentAssertions;
using FluentSerializer.Xml.Stories.OpenAir.Models;
using FluentSerializer.Xml.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles;
using System;
using FluentSerializer.Xml.Profiles;
using System.Linq;
using FluentSerializer.Xml.Stories.OpenAir.Models.Response;
using FluentSerializer.Xml.Stories.OpenAir.Models.Request;
using FluentSerializer.Xml.Mapping;

namespace FluentSerializer.Xml.Stories.OpenAir
{
    public partial class OpenAirTests
    {

        public OpenAirTests()
        {
            var profiles = new List<IXmlSerializerProfile>{
                new RequestProfile(),
                new ResponseProfile(),
                new ProjectProfile(),
                new RateCardProfile()
            };
            _mappings = profiles
                .SelectMany(x => x.Configure())
                .ToLookup(x => x.Key, x => x.Value);
        }

        private readonly ILookup<Type, XmlClassMap> _mappings;

        [Fact]
        public async Task Serialize()
        {
            // Arrange
            var expected = await File.ReadAllTextAsync("./OpenAirTests.Serialize.Xml");
            var example = new Request<Project>
            {
                AddRequests =
                {
                    new RequestObject<Project>
                    {
                        Data = new List<Project>
                        {
                            new Project
                            {
                                Id = "0001",
                                Active = true,
                                Name = "Project 1",
                                LastUpdate = DateTime.Parse("28-11-1991T3:00Z"),
                                CustomDate = DateTime.Parse("28-11-1991T3:00Z"),
                            }
                        }
                    },
                    new RequestObject<Project>{
                        Data = new List<Project>
                        {
                            new Project
                            {
                                Id = "0002",
                                Active = false,
                                Name = "Project 2",
                                LastUpdate = DateTime.Parse("28-11-1991T4:00Z")
                            },
                            new Project
                            {
                                Id = "0003",
                                Active = true,
                                Name = "Project 3",
                                LastUpdate = DateTime.Parse("28-11-1991T5:00Z")
                            }
                        }
                    }
                }
            };
            
            var sut = new FluentXmlSerializer(_mappings);

            // Act
            var result = sut.Serialize(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Deserialize()
        {
            // Arrange
            var expected = new Response<RateCard>();
            var example = await File.ReadAllTextAsync("./OpenAirTests.Deserialize.Xml");
            var sut = new FluentXmlSerializer(_mappings);

            // Act
            var result = sut.Deserialize<RateCard>(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
