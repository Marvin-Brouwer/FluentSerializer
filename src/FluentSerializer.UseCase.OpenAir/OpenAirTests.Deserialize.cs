using FluentSerializer.UseCase.OpenAir.Models;
using FluentSerializer.UseCase.OpenAir.Models.Response;

using System.Collections.Generic;

namespace FluentSerializer.UseCase.OpenAir;

public sealed partial class OpenAirTests
{
	private static readonly Response<RateCard> RateCardResponseExample = new()
	{
		ReadResponses = new List<ReadResponse<RateCard>>
		{
			new()
			{
				StatusCode = 0,
				Data = new()
				{
					new RateCard {
						Id = "RC1",
						Name = "Ratecard 1"
					},
					new RateCard {
						Id = "RC2",
						LastUpdate = CreateDate("1991-11-28 04:00:00")
					}
				}
			},
			new()
			{
				StatusCode = 601
			}
		}
	};
}