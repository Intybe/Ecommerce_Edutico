using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace edutico.Controllers
{
    public class HomeController : Controller

    {

        private readonly ILogger<HomeController> _logger;
        private IProdutoRepositorio? _produtoRepositorio;
        private IFavoritosRepositorio? _favoritosRepositorio;

        private readonly LoginSessao _loginSessao;

        public HomeController(ILogger<HomeController> logger, IProdutoRepositorio produtoRepositorio, LoginSessao loginSessao, IFavoritosRepositorio favoritosRepositorio)
        {
            _logger = logger;
            _produtoRepositorio = produtoRepositorio;
            _loginSessao = loginSessao;
            _favoritosRepositorio = favoritosRepositorio;
            
        }

     
        public IActionResult Index()
        {
            // Cria uma lista para armazenar vários produtos
            List<Produto> produtos = new List<Produto>();
            List<decimal> favoritos = new List<decimal>();

            // Verifica se o usuário está logado
            if (_loginSessao != null && _loginSessao.GetLogin() != null)
            {
                var login = _loginSessao.GetLogin(); // Obtém o login do usuário

                // Verificando se o nível de acesso é da manutenção
                if (login.nivelAcesso == 0)
                {
                    // Looping para passar 10 produtos
                    for (int i = 0; i < 10; i++)
                    {
                        produtos.Add(
                            new Produto
                            {
                                nomeProd = "Nome Produto",
                                valorUnit = 0,
                                imgs = new List<Imagem> { new Imagem { enderecoImg = "~/imgs/img_prod_padrao_quadrada.png" } }
                            }
                        );
                    }
                }
                else if (_produtoRepositorio.ConsultarProdutoLancamento() != null)
                {
                    // Busca os produtos no banco de dados e armazena na lista
                    produtos = _produtoRepositorio.ConsultarProdutoLancamento().ToList(); // Obtém os produtos

                  
                    // Obtém os favoritos do cliente logado
                    var favoritosList = _favoritosRepositorio.ConsultarFavoritos(login.codLogin).ToList();

                    // Extrai os códigos dos produtos favoritos
                    favoritos = favoritosList.Select(f => f.produto.codProd).ToList();
                }
            }
            else if (_produtoRepositorio.ConsultarProdutoLancamento() != null)
            {
                // Busca os produtos no banco de dados e armazena na lista
                produtos = _produtoRepositorio.ConsultarProdutoLancamento().ToList(); // Obtém os produtos

                // Aqui não foi definido o `codLogin` se o usuário não está logado. 
                // Talvez precise fazer um comportamento específico para usuário não logado.
            }

            var viewModel = new ViewProdutoFavoritos
            {
                produtos = produtos,
                favoritos = favoritos
            };

            return View(viewModel); // Passa o ViewModel para a view
        }

        [HttpPost]
        public IActionResult Favoritar(decimal codProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }
        
            _favoritosRepositorio.Favoritar(codLogin.codLogin, codProd);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult RemoverFavorito(decimal codProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }
          
            _favoritosRepositorio.RemoverFavorito(codLogin.codLogin, codProd);

            return Redirect(Request.Headers["Referer"].ToString());
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