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
using FluentSerializer.Core.Mapping;
using System.Text;
using FluentSerializer.Core.Configuration;

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
            _mappings = new ClassMapDictionary(profiles.SelectMany(x => x.Configure()));

            _configuration = ConfigurationConstants.GetDefaultXmlConfiguration();
            _configuration.Encoding = Encoding.UTF8;
        }

        private readonly ISearchDictionary<Type, IClassMap> _mappings;
        private readonly SerializerConfiguration _configuration;

        [Fact]
        public async Task Serialize()
        {
            // Arrange
            var expected = await File.ReadAllTextAsync("../../../OpenAir/OpenAirTests.Serialize.Xml");
            var example = new Request<Project>
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
                                LastUpdate = DateTime.Parse("1991-11-28T03:00:00.0000000Z"),
                                CustomDate = DateTime.Parse("1991-11-28T03:00:00.0000000Z"),
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
                                LastUpdate = DateTime.Parse("1991-11-28T04:00:00.0000000Z")
                            },
                            new Project
                            {
                                Id = "0003",
                                Active = true,
                                Name = "Project 3",
                                LastUpdate = DateTime.Parse("1991-11-28T05:00:00.0000000Z")
                            }
                        }
                    }
                }
            };

            var sut = new FluentXmlSerializer(_mappings, _configuration);

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
            var example = await File.ReadAllTextAsync("../../../OpenAir/OpenAirTests.Deserialize.Xml");
            var sut = new FluentXmlSerializer(_mappings, _configuration);

            // Act
            var result = sut.Deserialize<Response<RateCard>>(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
