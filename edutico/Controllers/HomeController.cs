using edutico.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace edutico.Controllers
{
    public class HomeController : Controller

    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

     

        public IActionResult BuscaCategoria()
        {
            return View();
        }

        public IActionResult CadastroCliente() 
        {
            return View();
        }

        public IActionResult EsqueceuSenha()
        {
            return View();
        }

        public IActionResult FavoritosCheio()
        {
            return View();
        }

        public IActionResult FavoritosVazio()
        {
            return View();
        }

        public IActionResult CarrinhoCheio() {
            return View();
        }

        public IActionResult CarrinhoVazio()
        {
            return View();
        }

        //FUNCIONÁRIO
        public IActionResult ProdutosF()
        {
            return View();
        }

        public IActionResult PedidosAndamentoF()
        {
            return View();
        }

        public IActionResult DetalhesPedidoF()
        {
            return View();
        }

        public IActionResult CadastrarProdutosF()
        {
            return View();
        }

        public IActionResult DetalhesProduto()
        {
            return View();
        }

        public IActionResult AlterarProdutosF()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}