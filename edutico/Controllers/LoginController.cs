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
                    return RedirectToAction("PedidosAndamentoF", "Home");
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

    }
}
