using edutico.Libraries.Login;
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
        private readonly LoginSessao _loginSessao;

        public HomeController(ILogger<HomeController> logger, IProdutoRepositorio produtoRepositorio, LoginSessao loginSessao)
        {
            _logger = logger;
            _produtoRepositorio = produtoRepositorio;
            _loginSessao = loginSessao;
        }

        public IActionResult Index()
        {
            // Cria uma lista para armazenar vários produtos
            List<Produto> produtos = new List<Produto>();

            // Verifica se o usuário está logado
            if (_loginSessao != null && _loginSessao.GetLogin() != null)
            {
                // Verificando se o nível de acesso é da manutenção
                if (_loginSessao.GetLogin().nivelAcesso == 0)
                {
                    // Looping para passar 10 produtos
                    for (int i = 0; i < 10; i++)
                    {
                        (produtos as List<Produto>).Add(
                            new Produto
                            {
                                nomeProd = "Nome Produto",
                                valorUnit = 0,
                                imgs = new List<Imagem> { new Imagem { enderecoImg = "~/imgs/img_prod_padrao_quadrada.png" } }
                            }
                        );
                    }
                }
                else if ((_produtoRepositorio.ConsultarProdutoLancamento()) != null)
                {
                    // Busca os produtos no banco de dados e armazena na lista
                    produtos = _produtoRepositorio.ConsultarProdutoLancamento().ToList(); // Obtém os produtos
                }
            }
            else if ((_produtoRepositorio.ConsultarProdutoLancamento()) != null)
            {
                // Busca os produtos no banco de dados e armazena na lista
                produtos = _produtoRepositorio.ConsultarProdutoLancamento().ToList(); // Obtém os produtos
            }

            return View(produtos); // Passa os produtos para a view
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

        public IActionResult AcompanharPedidos()
        {
            return View();
        }

        public IActionResult AcompanharPedidosVazio()
        {
            return View();
        }

        public IActionResult FinalizarPedido()
        {
            return View();
        }
       
        public IActionResult CartaoPagamento()
        {
            return View();
        }

        public IActionResult DetalhesPedido()
        {
            return View();
        }

        public IActionResult ProdutosDevolucao()
        {
            return View();
        }   

        public IActionResult CadastroMeusCartoes()
        {
            return View();
        }

        public IActionResult MeusCartoes()
        {
            return View();  
        }

        public IActionResult CancelamentoEfetuado()
        {
            return View();
        }

        public IActionResult FormDevolucao()
        {
            return View();
        }

        //FUNCIONÁRIO

        public IActionResult DashBoard()
        {
            return View();
        }
        public IActionResult ProdutosF()
        {
            return View();
        }

        public IActionResult PedidosAndamentoF()
        {
            return View();
        }

        public IActionResult DetalhesPedidoEmAndamentoF()
        {
            return View();
        }

        public IActionResult DetalhesPedidoConcluidoF()
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

        public IActionResult DevolucaoConcluidaF()
        {
            return View();
        }

        public IActionResult DevolucaoEmAndamentoF()
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