using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles
{
    public sealed class RequestProfile : JsonSerializerProfile
    {
        protected override void Configure()
        {
            For<Request<IMavenlinkEntity>>()
                .Property(project => project.Data);
        }
    }
}
