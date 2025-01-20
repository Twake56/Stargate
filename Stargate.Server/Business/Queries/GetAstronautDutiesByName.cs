using MediatR;
using Stargate.Server.Controllers;
using Stargate.Server.Data.Models;
using Stargate.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Stargate.Server.Business.Queries
{
    public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
    {
        private readonly StargateContext _context;

        public GetAstronautDutiesByNameHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetAstronautDutiesByNameResult> Handle(GetAstronautDutiesByName request, CancellationToken cancellationToken)
        {
            var result = new GetAstronautDutiesByNameResult();

            var person = await _context.PersonAstronauts.FromSql($"SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate, b.Id FROM [Person] AS a LEFT JOIN [AstronautDetail] AS b on b.PersonId = a.Id WHERE LOWER({request.Name}) = LOWER(a.Name)").FirstOrDefaultAsync();

            if (person is null)
            {
                result.Success = false;
                result.Message = "Astronaut not found";
                return result;
            }
            result.Person = person;

            var duties = await _context.AstronautDuties.FromSql($"SELECT * FROM [AstronautDuty] WHERE {person.PersonId} = PersonId Order By DutyStartDate Desc").ToListAsync();

            result.AstronautDuties = duties;
            

            return result;

        }
    }

    public class GetAstronautDutiesByNameResult : BaseResponse
    {
        public PersonAstronaut Person { get; set; }
        public List<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
    }
}
