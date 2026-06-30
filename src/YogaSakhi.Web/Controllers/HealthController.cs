using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using YogaSakhi.Application.CQRS.Queries.Health;

namespace YogaSakhi.Web.Controllers
{
    public class HealthController : Controller
    {
        private readonly IMediator _mediator;

        public HealthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _mediator.Send(new GetAllHealthConditionsQuery());
            return View(response);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _mediator.Send(new GetHealthConditionQuery { HealthConditionId = id });
            if (response == null)
                return NotFound();
            return View(response);
        }
    }
}
