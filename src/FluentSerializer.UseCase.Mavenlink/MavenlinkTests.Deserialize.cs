using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink
{
    public sealed partial class MavenlinkTests
    {
        private static readonly Response<User> UserResponseExample = new()
        {
            Count = 1,
			CurrentPage = 1,
			PageCount = 1,
            Data = new ()
            {
                new User
                {
                    Id = "U1",
					Name = "John Doe",
					Age = 22
				},
				new User
				{
					Id = "U2",
					Name = "Jane Doe",
					Age = 22
				}
			}
        };
    }
}
