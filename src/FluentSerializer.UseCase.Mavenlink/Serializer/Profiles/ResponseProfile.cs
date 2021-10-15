using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles
{
    public sealed class ResponseProfile : JsonSerializerProfile
    {
        protected override void Configure()
        {
            For<Response<IMavenlinkEntity>>()
                .Property(project => project.Count)
                .Property(project => project.Data);
        }
    }
}
