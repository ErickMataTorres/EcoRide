using Microsoft.AspNetCore.Mvc;

namespace EcoRide.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("UsuarioLogin")]
        public IActionResult UsuarioLogin()
        {
            return View();
        }
        [HttpPost]
        [Route("ValidarUsuario")]
        public IActionResult ValidarUsuario([FromBody]Models.Usuario c)
        {
            var r = c.ValidarUsuario();
            return Json(r);
        }
        [HttpPost]
        [Route("RegistrarUsuario")]
        public IActionResult RegistrarUsuario(Models.Usuario c)
        {
            var r = c.RegistrarUsuario();
            return Json(r);
        }
        [HttpDelete]
        [Route("BorrarUsuario/{id}")]
        public IActionResult BorrarUsuario(int id)
        {
            var r = Models.Usuario.BorrarUsuario(id);
            return Json(r);
        }
    }
}
