using Microsoft.AspNetCore.Mvc;
using edutico.Models;

public class LoginController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string usuario, string senha)
    {
        Login loginModel = new Login();

        // Chama o método para verificar o login no banco de dados
        bool loginValido = loginModel.login(usuario, senha);

        if (loginValido)
        {
            // Se o login for válido, armazene os dados do usuário (codLogin e nivelAcesso) na sessão
            HttpContext.Session.SetInt32("codLogin", loginModel.codLogin);
            HttpContext.Session.SetInt32("nivelAcesso", loginModel.nivelAcesso);

            // Redirecionar para a página com base no nível de acesso
            if (loginModel.nivelAcesso == 1)
            {
                return RedirectToAction("DetalhesPedidoF", "Home");
            }
            else if (loginModel.nivelAcesso == 2)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // Se o login falhar, defina a mensagem de erro e retorne a view de login
        ViewBag.Error = "Usuário ou senha incorretos!";
        return View("Index"); // Assegura que a view correta seja retornada
    }


}
