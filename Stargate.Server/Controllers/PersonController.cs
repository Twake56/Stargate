using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Server.Business.Commands;
using Stargate.Server.Business.Queries;
using System.Net;

namespace Stargate.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ILogger _logger;

        public PersonController(IMediator mediator, ILogger<PersonController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPeople()
        {            
            try
            {
                var result = await _mediator.Send(new GetPeople()
                {

                });
                _logger.LogInformation($"Get people : {result.People.Select(x => x.Name)} ");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred getting people");

                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPersonByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetPersonByName()
                {
                    Name = name
                });
                _logger.LogInformation($"Get person by name: {result.Person.Name} ");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred getting person by name");

                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePerson([FromBody] string name)
        {
            try
            {
                var result = await _mediator.Send(new CreatePerson()
                {
                    Name = name
                });
                _logger.LogInformation($"Created person: {result.Id}");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred creating person");

                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }

        }
    }
}
