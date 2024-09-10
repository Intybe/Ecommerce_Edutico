using Microsoft.AspNetCore.Mvc;

namespace edutico.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
