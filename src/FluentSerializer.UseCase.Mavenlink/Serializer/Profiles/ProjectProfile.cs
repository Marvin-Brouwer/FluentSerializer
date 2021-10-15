using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles
{
    public sealed class ProjectProfile : JsonSerializerProfile
    {
        protected override void Configure()
        {
            For<Project>()
                .Property(project => project.Id);
        }
    }
}
