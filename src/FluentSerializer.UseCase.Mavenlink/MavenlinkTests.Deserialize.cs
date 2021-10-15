using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink
{
    public sealed partial class MavenlinkTests
    {
        private static readonly Response<User> UserResponseExample = new()
        {
            Count = 1,
            Data = new ()
            {
                new User
                {
                    Id = "U1"
                }
            }
        };
    }
}
