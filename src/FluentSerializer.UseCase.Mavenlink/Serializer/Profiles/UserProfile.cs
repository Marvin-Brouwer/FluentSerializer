using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.Profiles
{
    public sealed class UserProfile : JsonSerializerProfile
    {
        protected override void Configure()
        {
            For<User>()
                .Property(project => project.Id);
        }
    }
}
