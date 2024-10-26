using edutico.Libraries.Login;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using edutico.Models;
using Newtonsoft.Json;

namespace edutico.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly LoginSessao _loginSessao;
        private IClienteRepositorio? _clienteRepositorio;
        private ICartaoRepositorio? _cartaoRepositorio;
        private IProdutoRepositorio? _produtoRepositorio;

        public PedidoController(IPedidoRepositorio pedidoRepositorio, LoginSessao loginSessao, IClienteRepositorio clienteRepositorio, ICartaoRepositorio? cartaoRepositorio, IProdutoRepositorio? produtoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _loginSessao = loginSessao;
            _clienteRepositorio = clienteRepositorio;
            _cartaoRepositorio = cartaoRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        // Método para exibir a página de finalizar o pedido
        [HttpPost]
        public IActionResult FinalizarPedido(string lista, double valorTotal)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            // Chama o método do repositório para consultar o cliente no banco de dados
            Cliente cliente = _clienteRepositorio.ConsultarCliente(Login.codLogin);

            var listaProdutos = JsonConvert.DeserializeObject<List<Carrinho>>(lista);

            // Criar uma instância do Pedido
            var pedido = new Pedido
            {
                cliente = cliente,
                itensPedido = new List<ItemPedido>(),
                valorTotal = Convert.ToDecimal(valorTotal)
            };

            // Fragmentar os itens do carrinho em itens do pedido
            foreach (var item in listaProdutos)
            {
                var itemPedido = new ItemPedido
                {
                    produto = item.produto,
                    qtdItem = item.qtdProd,
                    valorItem = item.produto.valorUnit * item.qtdProd // Calcular o valor do item
                };

                pedido.itensPedido.Add(itemPedido);
            }

            return View(pedido);
        }

        // Método POST para cadastrar o pedido e os itens
        [HttpPost]
        public IActionResult CadastrarPedido(string pedidoIn, int tipoPag)
        {
            // Desserializa o JSON recebido de volta para um objeto Cartao
            var pedido = JsonConvert.DeserializeObject<Pedido>(pedidoIn);

            // Chama o método do repositório para cadastrar o pedido e seus itens
            string resultado = _pedidoRepositorio.CadastrarPedido(pedido);

            // Tenta converter o resultado para int
            if (int.TryParse(resultado, out _))
            {
                pedido.NF = Convert.ToInt32(resultado);

                if (tipoPag == 0)
                {
                    // Gerando o código PIX no formato GUID
                    var codigoPix = Guid.NewGuid().ToString();

                    // Retorno Aleatório
                    var retornoPag = new Random().Next(0, 2) == 1;

                    Pagamento pagamento = new Pagamento()
                    {
                        qtdParcela = 1,
                        codPix = codigoPix,
                        pedido = pedido,
                        retorno = retornoPag
                    };

                    TempData["Pagamento"] = JsonConvert.SerializeObject(pagamento);
                    return RedirectToAction("PagamentoQrcode", "Pagamento");
                }
                else
                {
                    // Pega o codLogin do Usuário Logado através da sessão
                    var Login = _loginSessao.GetLogin();

                    List<Cartao> cartoes = new List<Cartao>();

                    // Consulta a lista de cartões cadastrados para o usuário logado
                    cartoes = _cartaoRepositorio.ConsultarCartao(Login.codLogin);

                    Pagamento pagamento = new Pagamento()
                    {
                        pedido = pedido,
                        cartoes = cartoes
                    };

                    TempData["Pagamento"] = JsonConvert.SerializeObject(pagamento);
                    return RedirectToAction("PagamentoCartao", "Pagamento");
                }
            }
            else
            {
                // Se o ModelState não for válido, retorna à página de finalização de pedido
                return View("FinalizarPedido", pedido);
            }
        }


        public IActionResult MeusPedidos()
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            if (Login == null)
            {
                // Se o cliente não estiver logado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }

            List<Pedido> pedidos = _pedidoRepositorio.ConsultarPedidos(Login.codLogin);


            foreach (var pedido in pedidos)
            {
                foreach (var item in pedido.itensPedido)
                {
                    // Chama o método para obter os detalhes completos do produto
                    Produto produtoCompleto = _produtoRepositorio.ConsultarDetalheProduto(item.produto.codProd);

                    // Atualiza o objeto produto do item com os dados completos
                    item.produto = produtoCompleto;
                }
            }

            return View(pedidos);
        }

        public IActionResult ConsultarPedidosFiltros(int statusPedido)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            List<Pedido> pedidos = null;

            if (statusPedido == 5)
            {
                pedidos = _pedidoRepositorio.ConsultarPedidos(Login.codLogin);

                foreach (var pedido in pedidos)
                {
                    foreach (var item in pedido.itensPedido)
                    {
                        // Chama o método para obter os detalhes completos do produto
                        Produto produtoCompleto = _produtoRepositorio.ConsultarDetalheProduto(item.produto.codProd);

                        // Atualiza o objeto produto do item com os dados completos
                        item.produto = produtoCompleto;
                    }
                }

                return View(pedidos);
            }
            else
            {
                pedidos = _pedidoRepositorio.ConsultarPedidosFiltros(Login.codLogin, statusPedido);

                foreach (var pedido in pedidos)
                {
                    foreach (var item in pedido.itensPedido)
                    {
                        // Chama o método para obter os detalhes completos do produto
                        Produto produtoCompleto = _produtoRepositorio.ConsultarDetalheProduto(item.produto.codProd);

                        // Atualiza o objeto produto do item com os dados completos
                        item.produto = produtoCompleto;
                    }
                }

                return View("MeusPedidos", pedidos);
            }
        }
    }
}

