using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace edutico.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteRepositorio? _clienteRepositorio;

        public ClienteController(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult CadastroCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarCliente(decimal cpf, string nome, string sobrenome, string telefone, string email, string logradouro, string bairro, string cidade, string uf, string cep, int numEnd, string compEnd, string senha)
        {
            string mensagem = _clienteRepositorio.CadastrarCliente(cpf, nome, sobrenome, telefone, email, logradouro, bairro, cidade, uf, cep, numEnd, compEnd, senha);
            // ViewBag.ConsoleMessage = mensagem;

            ViewData["msg"] = mensagem;
            return View("CadastroCliente");
        }
    }
}
