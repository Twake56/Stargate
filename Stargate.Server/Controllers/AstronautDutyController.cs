using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Server.Business.Commands;
using Stargate.Server.Business.Queries;
using System.Net;

namespace Stargate.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ILogger _logger;

        public AstronautDutyController(IMediator mediator, ILogger<AstronautDutyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAstronautDutiesByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetAstronautDutiesByName()
                {
                    Name = name
                });
                _logger.LogInformation($"Astronaut Duties by name returned Name: {result.Person} ");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred getting astronaut duties by name");

                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
        {
            var result = await _mediator.Send(request);
            _logger.LogInformation($"CreateAstronautDuty: {result.Id}");
            return this.GetResponse(result);
        }
    }
}
