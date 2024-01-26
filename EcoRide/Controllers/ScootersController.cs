using Microsoft.AspNetCore.Mvc;

namespace EcoRide.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScootersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("ConsultarScooters")]
        public IActionResult ConsultarScooters()
        {
            var r = Models.Scooter.ConsultarScooters();
            return Json(r);
        }
    }
}
