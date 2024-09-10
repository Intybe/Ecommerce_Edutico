using Microsoft.AspNetCore.Mvc;

namespace edutico.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult CadastroCliente()
        {
            return View();
        }


    }
}
