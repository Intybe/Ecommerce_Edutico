using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace edutico.Controllers
{
    public class HomeController : Controller

    {

        private readonly ILogger<HomeController> _logger;
        private IProdutoRepositorio? _produtoRepositorio;

        public HomeController(ILogger<HomeController> logger, IProdutoRepositorio produtoRepositorio)
        {
            _logger = logger;
            _produtoRepositorio = produtoRepositorio;
        }

        public IActionResult Index()
        {
            IEnumerable<Produto> produto = null;

            if ((_produtoRepositorio.ConsultarProdutoLancamento()) != null)
            {
                produto = _produtoRepositorio.ConsultarProdutoLancamento(); // Obtém os produtos
            }

            return View(produto); // Passa os produtos para a view
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

        public IActionResult DetalheProduto()
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

        public IActionResult CarrinhoCheio()
        {
            return View();
        }

        public IActionResult CarrinhoVazio()
        {
            return View();
        }

        public IActionResult PagamentoQrcode()
        {
            return View();
        }

        public IActionResult PagamentoCartao()
        {
            return View();
        }

        public IActionResult TermoDevolucao()
        {
            return View();
        }

        public IActionResult TermoCancelamento()
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

        public IActionResult AlterarProdutosF()
        {
            return View();
        }

        public IActionResult FavoritosF()
        {
            return View();
        }

        public IActionResult HistoricoVendasF()
        {
            return View();
        }

        public IActionResult HistoricoVendasInfoF()
        {
            return View();
        }

        public IActionResult DetalhesProdutoF()
        {
            return View();
        }

        public IActionResult DevolucoesF()
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