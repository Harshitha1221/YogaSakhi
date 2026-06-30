using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using YogaSakhi.Application.CQRS.Commands.Auth;
using YogaSakhi.Application.CQRS.Queries.Health;
using YogaSakhi.Application.CQRS.Queries.AI;
using YogaSakhi.Application.CQRS.Commands.Health;

namespace YogaSakhi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
                return Unauthorized(response);
            return Ok(response);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HealthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("conditions")]
        public async Task<IActionResult> GetAllConditions()
        {
            var response = await _mediator.Send(new GetAllHealthConditionsQuery());
            return Ok(response);
        }

        [HttpGet("conditions/{id}")]
        public async Task<IActionResult> GetCondition(int id)
        {
            var response = await _mediator.Send(new GetHealthConditionQuery { HealthConditionId = id });
            if (response == null)
                return NotFound();
            return Ok(response);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssessmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAssessment([FromBody] CreateAssessmentCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserAssessments(int userId)
        {
            var response = await _mediator.Send(new GetUserAssessmentQuery { UserId = userId });
            return Ok(response);
        }

        [HttpPost("record-progress")]
        public async Task<IActionResult> RecordProgress([FromBody] RecordProgressCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AIController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("analyze-symptoms")]
        public async Task<IActionResult> AnalyzeSymptoms([FromBody] AnalyzeSymptomsQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("recommendation/{userId}/{conditionId}")]
        public async Task<IActionResult> GetRecommendation(int userId, int conditionId)
        {
            var response = await _mediator.Send(new GetAIRecommendationQuery 
            { 
                UserId = userId, 
                HealthConditionId = conditionId 
            });
            return Ok(response);
        }
    }
}
