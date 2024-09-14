using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace edutico.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteRepositorio? _clienteRepositorio;
        private readonly LoginSessao _loginSessao;

        // Construtor com injeção de dependência
        public ClienteController(IClienteRepositorio clienteRepositorio, LoginSessao loginSessao)
        {
            _clienteRepositorio = clienteRepositorio;
            _loginSessao = loginSessao; // Inicializa a variável _loginSessao
        }

        public IActionResult CadastroCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarCliente(decimal cpf, string nome, string sobrenome, string telefone, string email, string logradouro, string bairro, string cidade, string uf, string cep, int numEnd, string compEnd, string senha)
        {
            string mensagem = _clienteRepositorio.CadastrarCliente(cpf, nome, sobrenome, telefone, email, logradouro, bairro, cidade, uf, cep, numEnd, compEnd, senha);

            ViewData["msg"] = mensagem;
            return View("CadastroCliente");
        }

        public IActionResult MeuPerfil()
        {
            // Obtém o cliente logado através da sessão
            var login = _loginSessao.GetLogin();

            if (login == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            // Chama o método do repositório para consultar o cliente no banco de dados
            Cliente cliente = _clienteRepositorio.ConsultarCliente(login.codLogin);

            // Passa os dados do cliente para a view
            return View(cliente);
        }
    }
}
