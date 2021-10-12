using System.Collections.Generic;
using FluentSerializer.UseCase.OpenAir.Models;
using FluentSerializer.UseCase.OpenAir.Models.Response;

namespace FluentSerializer.UseCase.OpenAir
{
    public sealed partial class OpenAirTests
    {
        private static readonly Response<RateCard> RateCardResponseExample = new Response<RateCard>
        {
            ReadResponses = new List<ReadResponse<RateCard>>
            {
                new ReadResponse<RateCard>
                {
                    StatusCode = 0,
                    Data = new List<RateCard>
                    {
                        new RateCard{
                            Id = "RC1",
                            Name = "Ratecard 1"
                        },
                        new RateCard{
                            Id = "RC2",
                            LastUpdate = CreateDate("1991-11-28 04:00:00")
                        }
                    }
                },
                new ReadResponse<RateCard>
                {
                    StatusCode = 601
                }
            }
        };
    }
}
