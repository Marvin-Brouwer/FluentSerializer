using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.DependencyInjection.NetCoreDefault.Extensions;
using FluentSerializer.Json.Converter.DefaultJson.Extensions;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Services;
using FluentSerializer.UseCase.Mavenlink.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FluentSerializer.UseCase.Mavenlink
{
	public sealed partial class MavenlinkTests
    {
		private readonly IServiceProvider _serviceProvider;

		public MavenlinkTests()
        {
			_serviceProvider = new ServiceCollection()
				.AddFluentJsonSerializer<MavenlinkTests>(static configuration =>
				{
					configuration.DefaultNamingStrategy = Names.Use.SnakeCase;
					configuration.DefaultConverters.Add(Converter.For.Json());
					configuration.NewLine = LineEndings.LineFeed;
				})
				.BuildServiceProvider();
		}

        [Fact,
            Trait("Category", "UseCase")]
        public async Task Serialize()
        {
            // Arrange
            var expected = await File.ReadAllTextAsync("./MavenlinkTests.Serialize.json");
            var example = ProjectRequestExample;

            var sut = _serviceProvider.GetService<RuntimeJsonSerializer>()!;

            // Act
            var result = sut.Serialize(example);

            // Assert
            result.ShouldBeBinaryEquatableTo(expected);
        }

        [Fact,
            Trait("Category", "UseCase")]
        public async Task Deserialize()
        {
            // Arrange
            var expected = UserResponseExample;
            var example = await File.ReadAllTextAsync("./MavenlinkTests.Deserialize.json");

			var sut = _serviceProvider.GetService<RuntimeJsonSerializer>()!;

			// Act
			var result = sut.Deserialize<Response<User>>(example);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        private static DateTime CreateDate(string dateString) => DateTime.ParseExact(
            dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
    }
}