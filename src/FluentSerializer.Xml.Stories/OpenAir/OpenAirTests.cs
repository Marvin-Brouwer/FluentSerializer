using FluentAssertions;
using FluentSerializer.Xml.Stories.OpenAir.Models;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles;

namespace FluentSerializer.Xml.Tests
{
    public partial class OpenAirTests
    {
        private readonly List<XmlSerializerProfile> _profiles = new List<XmlSerializerProfile>
        {
            new RequestProfile(),
            new ResponseProfile(),
            new ProjectProfile(),
            new RateCardProfile()
        };

        [Fact]
        public async Task Serialize()
        {
            // Arrange
            var expected = await File.ReadAllTextAsync("./Serialize.Xml");
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
                                Name = "Project 1"
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
                                Name = "Project 2"
                            },
                            new Project
                            {
                                Id = "0003",
                                Active = true,
                                Name = "Project 3"
                            }
                        }
                    }
                }
            };
            
            var sut = new FluentXmlSerializer(_profiles);

            // Act
            var result = sut.Serialize(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Deserialize()
        {
            // Arrange
            var expected = new Response<Project>();
            var example = await File.ReadAllTextAsync("./Deserialize.Xml");
            var sut = new FluentXmlSerializer(_profiles);

            // Act
            var result = sut.Deserialize<RateCard>(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
