using Microsoft.AspNetCore.Mvc;

namespace EcoRide.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("Rentas")]
        public IActionResult Rentas()
        {
            return View();
        }
        [HttpGet]
        [Route("TablaTemporalRentas")]
        public IActionResult TablaTemporalRentas()
        {
            var r = Models.TemporalRenta.TablaTemporalRentas();
            return Json(r);
        }
        [HttpPost]
        [Route("PostTemporalRentas")]
        public IActionResult PostTemporalRentas([FromBody] Models.TemporalDatos scooterSeleccionado) 
        {
            return Json(new Models.TemporalRenta().PostTemporalRentas(scooterSeleccionado));
        }
        [HttpDelete]
        [Route("BorrarTemporalRentas/{idScooter}")]
        public IActionResult BorrarTemporalRentas(int idScooter)
        {
            var r = Models.TemporalRenta.BorrarTemporalRentas(idScooter);
            return Json(r);
        }
        [HttpPost]
        [Route("ProcesarPago")]
        public IActionResult ProcesarPago([FromBody] Models.Renta c)
        {
            var r = c.ProcesarPago();
            return Json(r);
        } 
    }
}
