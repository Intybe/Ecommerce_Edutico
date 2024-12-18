﻿using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using System.Security.Claims;

namespace edutico.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteRepositorio? _clienteRepositorio;
        private ICarrinhoRepositorio? _carrinhoRepositorio;
        private IFavoritosRepositorio? _favoritosRepositorio;
        private readonly LoginSessao _loginSessao;

        // Construtor com injeção de dependência
        public ClienteController(IClienteRepositorio clienteRepositorio, LoginSessao loginSessao, ICarrinhoRepositorio carrinhoRepositorio, IFavoritosRepositorio favoritosRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _loginSessao = loginSessao; // Inicializa a variável _loginSessao
            _carrinhoRepositorio = carrinhoRepositorio;
            _favoritosRepositorio = favoritosRepositorio;
        }

        [HttpPost]
        public IActionResult CadastroCliente(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                // Algo deu errado, retorne a View com os erros
                return View(cliente);
            }
            return RedirectToAction("CadastroCliente");
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
            return View("MeuPerfil", cliente);
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

            return RedirectToAction("MeuPerfil");
        }

        [HttpPost]
        public IActionResult AtualizarClienteDados(string cpf, string nome, string sobrenome, string telefone)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            if (codLogin == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            string mensagem = _clienteRepositorio.AtualizarClienteDados(codLogin.codLogin, Convert.ToInt64(cpf), nome, sobrenome, telefone);

            ViewData["msg"] = mensagem;

            return RedirectToAction("MeuPerfil");
        }

        [HttpPost]
        public IActionResult AtualizarClienteEndereco(string logradouro, string bairro, string cep, string cidade, string uf, string numEnd, string compEnd)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var codLogin = _loginSessao.GetLogin();

            string mensagem = _clienteRepositorio.AtualizarClienteEndereco(codLogin.codLogin, logradouro, bairro, cep, cidade, uf, Convert.ToInt32(numEnd), compEnd);

            ViewData["msg"] = mensagem;

            return RedirectToAction("MeuPerfil");
        }
    }

}
