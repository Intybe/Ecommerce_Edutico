using edutico.Data;
using edutico.Libraries.Login;
using edutico.Models;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace edutico.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly ILogger<FuncionarioController> _logger;
        private IProdutoRepositorio? _produtoRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly LoginSessao _loginSessao;
        private IClienteRepositorio? _clienteRepositorio;

        public FuncionarioController(ILogger<FuncionarioController> logger, IProdutoRepositorio produtoRepositorio, IPedidoRepositorio pedidoRepositorio, LoginSessao loginSessao, IClienteRepositorio clienteRepositorio)
        {
            _logger = logger;
            _produtoRepositorio = produtoRepositorio;
            _pedidoRepositorio = pedidoRepositorio;
            _loginSessao = loginSessao;
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult ProdutosF()
        {
            List<Produto> produtos = new List<Produto>();

            // Verifica se o usuário está logado
            if (_loginSessao != null && _loginSessao.GetLogin() != null)
            {
                var login = _loginSessao.GetLogin(); // Obtém o login do usuário

                // Verificando se o nível de acesso é da manutenção
                if (login.nivelAcesso == 0)
                {
                    // Looping para passar 10 produtos
                    for (int i = 0; i < 10; i++)
                    {
                        produtos.Add(
                            new Produto
                            {
                                nomeProd = "Nome Produto",
                                valorUnit = 0,
                                imgs = new List<Imagem> { new Imagem { enderecoImg = "~/imgs/img_prod_padrao_quadrada.png" } }
                            }
                        );
                    }
                }
                else if (_produtoRepositorio.ConsultarProdutoF() != null)
                {
                    // Busca os produtos no banco de dados e armazena na lista
                    produtos = _produtoRepositorio.ConsultarProdutoF().ToList(); // Obtém os produtos
                }
            }
            else if (_produtoRepositorio.ConsultarProdutoF() != null)
            {
                // Busca os produtos no banco de dados e armazena na lista
                produtos = _produtoRepositorio.ConsultarProdutoF().ToList(); // Obtém os produtos

            }
            return View(produtos);
        }

        public IActionResult AtualizarStatusProduto(decimal codProd, int statusProd)
        {
            _produtoRepositorio.AtualizarStatusProduto(codProd, statusProd);

            List<Produto> produtos = _produtoRepositorio.ConsultarProdutoF().ToList();

            return View("ProdutosF", produtos);
        }

        public IActionResult AlterarProdutosF(decimal codProd)
        {
            //puxando informaões    
            // Consultar o produto pelo código
            var produto = _produtoRepositorio.ConsultarDetalheProduto(codProd);

            // Simulando a lista de categorias (você pode buscar isso do banco de dados)

            return View(produto);
        }

        public IActionResult Dashboard()
        {
            var dashboard = _produtoRepositorio.ConsultarInfosEcommerce();

            return View(dashboard);
        }

        public IActionResult Pedidos()
        {

            List<Pedido> pedidos = _pedidoRepositorio.ConsultarPedidos();

            foreach (var pedido in pedidos)
            {
                pedido.cliente = _clienteRepositorio.ConsultarCliente(pedido.cliente.codLogin);
            }

            return View(pedidos);
        }

        public IActionResult Favoritados()
        {
            List<Produto> produtos = _produtoRepositorio.ConsultarProdutosFavoritos();

            return View(produtos);
        }
    }
}
