using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;

namespace FluentSerializer.UseCase.Mavenlink;

public sealed partial class MavenlinkTests
{

	private static readonly User JohnDoe = new()
	{
		Id = "U1",
		Name = "John Doe",
		Age = 22,
		AccountMembershipId = "UA1",
		AccountMembership = new()
		{
			Id = "UA1",
			LineManagerId = "U2"
		}
	};

	private static readonly User JaneDoe = new()
	{
		Id = "U2",
		Name = "Jane Doe",
		Age = 22,
		AccountMembershipId = "UA2",
		AccountMembership = new ()
		{
			Id = "UA2",
			LineManagerId = null
		}
	};

	private static readonly User JanetDoe = new()
	{
		Id = "U3",
		Name = "Janet Doe",
		Age = 22,
		AccountMembershipId = "UA3",
		AccountMembership = new()
		{
			Id = "UA3",
			LineManagerId = "U5"
		}
	};

	private static readonly Response<User> UserResponseExample = new()
	{
		Count = 1,
		CurrentPage = 1,
		PageCount = 1,
		Data = new ()
		{
			JohnDoe,
			JaneDoe,
			JanetDoe
		}
	};
}