using Microsoft.AspNetCore.Mvc;

namespace EcoRide.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InicioController : Controller
    {
        [Route("/")]
        public IActionResult Inicio()
        {
            return View();
        }
    }
}
