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
        public IActionResult CadastrarCartao(decimal NumCartao, string NomeTitular, int DataVali, int Bandeira, int CodLogin)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _cartaoRepositorio.CadastrarCartao(NumCartao, NomeTitular, DataVali, Bandeira, CodLogin);

            ViewData["msg"] = mensagem;
            return RedirectToAction("MeusCartoes", "Home");
        }
    }
}
