using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;

namespace edutico.Controllers
{
    public class CartaoController : Controller
    {
        private IClienteRepositorio? _clienteRepositorio;
        private ICartaoRepositorio? _cartaoRepositorio;
        private readonly LoginSessao _loginSessao;


        public CartaoController(IClienteRepositorio clienteRepositorio, LoginSessao loginSessao, ICartaoRepositorio cartaoRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _loginSessao = loginSessao; // Inicializa a variável _loginSessao
            _cartaoRepositorio = cartaoRepositorio;
        }

        public IActionResult CadastroCartao()
        {
            return View();
        }


        public IActionResult CadastrarCartao(decimal numCartao, string nomeTitular, string dataVali, string bandeira)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _cartaoRepositorio.CadastrarCartao(numCartao, nomeTitular, dataVali, bandeira, codLogin.codLogin);

            ViewData["msg"] = mensagem;
            return RedirectToAction("MeusCartoes");
        }
        public IActionResult MeusCartoes()
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            List<Cartao> cartoes = new List<Cartao>();

            // Consulta a lista de cartões cadastrados para o usuário logado
            cartoes = _cartaoRepositorio.ConsultarCartao(codLogin.codLogin);

            // Passa a lista de cartões para a view
            return View(cartoes);

        }
    }
}
