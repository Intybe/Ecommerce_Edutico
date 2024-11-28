using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace edutico.Controllers
{
    public class CarrinhoController : Controller
    {
        private IClienteRepositorio? _clienteRepositorio;
        private ICarrinhoRepositorio? _carrinhoRepositorio;
        private readonly LoginSessao _loginSessao;

        // Construtor com injeção de dependência
        public CarrinhoController(IClienteRepositorio clienteRepositorio, LoginSessao loginSessao, ICarrinhoRepositorio carrinhoRepositorio, IFavoritosRepositorio favoritosRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _loginSessao = loginSessao; 
            _carrinhoRepositorio = carrinhoRepositorio;
        }


        // Método para consultar os itens do Carrinho do Cliente e abrir a tela Carrinho
        public IActionResult Carrinho()
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            if (Login == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }
           
            IEnumerable<Carrinho> carrinho = _carrinhoRepositorio.ConsultarCarrinho(Login.codLogin);

            return View("Carrinho", carrinho);
        }


        // Método para cadastrar os produtos no Carrinho
        public IActionResult CadastrarProdutoCarrinho(decimal codProd, int qtdProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            if (Login == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _carrinhoRepositorio.CadastrarProdutoCarrinho(Login.codLogin, codProd, qtdProd);

            TempData["msg"] = mensagem;
            return Redirect(Request.Headers["Referer"].ToString());
        }


        // Método para Atualizar do item do Cartinho
        [HttpPost]
        public IActionResult AtualizarQtdProdCarrinho(decimal codProd, int qtdProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            // Atualiza a quantidade de produtos no banco de dados
            string mensagem = _carrinhoRepositorio.AtualizarQtdProdCarrinho(codLogin.codLogin, codProd, qtdProd);

            // Exibe uma mensagem de sucesso ou erro
            ViewData["msg"] = mensagem;

            // Redireciona de volta para a página do carrinho
            return RedirectToAction("Carrinho");
        }


        // Método para excluir o produto do Carrinho
        [HttpPost]
        public IActionResult ExcluirItemProduto(decimal codProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            if (Login == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _carrinhoRepositorio.ExcluirItemCarrinho(Login.codLogin, codProd);

            ViewData["msg"] = mensagem;
            return RedirectToAction("Carrinho");
        }
    }
}
