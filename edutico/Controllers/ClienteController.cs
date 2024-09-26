using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;

namespace edutico.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteRepositorio? _clienteRepositorio;
        private ICarrinhoRepositorio? _carrinhoRepositorio;
        private readonly LoginSessao _loginSessao;

        // Construtor com injeção de dependência
        public ClienteController(IClienteRepositorio clienteRepositorio, LoginSessao loginSessao, ICarrinhoRepositorio carrinhoRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _loginSessao = loginSessao; // Inicializa a variável _loginSessao
            _carrinhoRepositorio = carrinhoRepositorio;
        }

        public IActionResult CadastroCliente()
        {
            return View();
        }

        public IActionResult MeuPerfil()
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            // Chama o método do repositório para consultar o cliente no banco de dados
            Cliente cliente = _clienteRepositorio.ConsultarCliente(codLogin.codLogin);

            // Passa os dados do cliente para a view
            return View(cliente);
        }

        [HttpPost]
        public IActionResult CadastrarCliente(decimal cpf, string nome, string sobrenome, string telefone, string email, string logradouro, string bairro, string cidade, string uf, string cep, int numEnd, string compEnd, string senha)
        {
            string mensagem = _clienteRepositorio.CadastrarCliente(cpf, nome, sobrenome, telefone, email, logradouro, bairro, cidade, uf, cep, numEnd, compEnd, senha);

            ViewData["msg"] = mensagem;
            return View("CadastroCliente");
        }

        [HttpPost]
        public IActionResult AtualizarClienteConta(string email, string senha)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            string mensagem = _clienteRepositorio.AtualizarClienteConta(codLogin.codLogin, email, senha);

            ViewData["msg"] = mensagem;
            return View("MeuPerfil");
        }

        [HttpPost]
        public IActionResult AtualizarClienteDados(decimal cpf, string nome, string sobrenome, string telefone)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _clienteRepositorio.AtualizarClienteDados(codLogin.codLogin, cpf, nome, sobrenome, telefone);

            ViewData["msg"] = mensagem;
            return View("MeuPerfil");
        }

        [HttpPost]
        public IActionResult AtualizarClienteEndereco(string logradouro, string bairro, string cep, string cidade, string uf, string numEnd, string compEnd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            string mensagem = _clienteRepositorio.AtualizarClienteEndereco(codLogin.codLogin, logradouro, bairro, cep, cidade, uf, Convert.ToInt32(numEnd), compEnd);

            ViewData["msg"] = mensagem;
            return View("MeuPerfil");
        }

        public IActionResult CadastrarProdutoCarrinho(decimal codProd, int qtdProd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _carrinhoRepositorio.CadastrarProdutoCarrinho(codLogin.codLogin, codProd, qtdProd);

            ViewData["msg"] = mensagem;
            return RedirectToAction("Index", "Home");
        }

        public IActionResult CarrinhoCheio()
        {
            return View();
        }

        public IActionResult CarrinhoVazio()
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            var retorno = _carrinhoRepositorio.ConsultarCarrinho(codLogin.codLogin);

            if (retorno != null)
            {
                IEnumerable<Carrinho> carrinho = _carrinhoRepositorio.ConsultarCarrinho(codLogin.codLogin);
                return View("CarrinhoCheio", carrinho);
            }
            else
            {
                return View();
            }
        }

    }
}
