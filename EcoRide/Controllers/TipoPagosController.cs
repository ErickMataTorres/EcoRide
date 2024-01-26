using Microsoft.AspNetCore.Mvc;

namespace EcoRide.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoPagosController : Controller
    {
        [Route("ControlTipoPagos")]
        public IActionResult TipoPagos()
        {
            return View();
        }
        [HttpGet]
        [Route("TablaTipoPagos")]
        public IActionResult TablaTipoPagos()
        {
            var r = Models.TipoPago.TablaTipoPagos();
            return Json(r);
        }
        [HttpPost]
        [Route("RegistrarTipoPagos")]
        public IActionResult RegistrarTipoPagos([FromBody]Models.TipoPago c)
        {
            var r = c.RegistrarTipoPagos();
            return Json(r);
        }
    }
}
