using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using YogaSakhi.Application.CQRS.Queries.Health;

namespace YogaSakhi.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var conditions = await _mediator.Send(new GetAllHealthConditionsQuery());
            return View(conditions);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
