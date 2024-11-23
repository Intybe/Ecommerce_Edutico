using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace edutico.Controllers
{
    public class LoginController : Controller
    {

        private ILoginRepositorio? _loginRepositorio;
        private LoginSessao _loginSessao;

        public LoginController(ILoginRepositorio loginRepositorio, LoginSessao loginSessao)
        {
            _loginRepositorio = loginRepositorio;
            _loginSessao = loginSessao;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login login)
        {
            Login loginDB = _loginRepositorio.Login(login.usuario, login.senha);

            if (loginDB != null)
            {
                if (loginDB.nivelAcesso == 0 || loginDB.nivelAcesso == 1)
                {
                    _loginSessao.Login(loginDB);
                    return RedirectToAction("Dashboard", "Funcionario");
                }
                else
                {
                    _loginSessao.Login(loginDB);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // Erro na sessão
                ViewData["msg"] = "Usuário ou senha inválidos";
                return View();
            }

        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Remove todos os dados da sessão
            HttpContext.Session.Clear();

            // Redireciona para a tela de login
            return RedirectToAction("Login", "Login");
        }

 
        public IActionResult AlterarSenha(string novaSenha, string confirmarSenha, string usuario)
        {
            if (novaSenha != confirmarSenha)
            {
                ViewData["msg"] = "As senhas não coincidem.";
                return View();
            }
                // Obter o codLogin pelo e-mail
                decimal codLogin = _loginRepositorio.ObterCodLoginPorUsuario(usuario);

                // Chama o método do repositório para alterar a senha
                _loginRepositorio.AlterarSenha(codLogin, novaSenha);

                return RedirectToAction("Login");
        }

        public IActionResult EsqueceuSenha()
        {
            return View();
        }

    }
}
