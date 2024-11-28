using edutico.Libraries.Login;
using edutico.Repositorio;
using Microsoft.AspNetCore.Mvc;
using edutico.Models;
using Newtonsoft.Json;
using Aspose.Words;
using System.IO;
using Aspose.Words.Replacing;
using Aspose.Words.Tables;

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

                    var retornoPag = new Random().Next(0, 2) == 1;
                    // Consulta a lista de cartões cadastrados para o usuário logado
                    cartoes = _cartaoRepositorio.ConsultarCartao(Login.codLogin);

                    Pagamento pagamento = new Pagamento()
                    {
                        pedido = pedido,
                        cartoes = cartoes,
                        retorno = retornoPag
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

        public IActionResult FiltrarPedidos(int statusPedido)
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
        public IActionResult DetalhesPedido(int NF)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            Pedido pedido = _pedidoRepositorio.ConsultarDetalhesPedido(NF);

            // Adiciona os dados do cliente ao pedido
            pedido.cliente = _clienteRepositorio.ConsultarCliente(Login.codLogin);

            return View(pedido);
        }


        public IActionResult GerarNF(string pedido)
        {
            // Desserializa o pedido
            var dadosPedido = JsonConvert.DeserializeObject<Pedido>(pedido);

            // Armazena o caminho do template
            string caminhoTemplate = "wwwroot/template/templateNF.docx";

            // Transforma o arquivo original em bytes
            byte[] arquivoTemplate = System.IO.File.ReadAllBytes(caminhoTemplate);

            // Cria uma cópia arquivo na memória, pois não podemos alterar o template diretamente
            MemoryStream arquivoMemoria = new MemoryStream(arquivoTemplate);

            // Abre o arquivo da memória em Word
            Document arquivoWord = new Document(arquivoMemoria);

            // Editando o arquivo
            arquivoWord.Range.Replace("{{nomeCli}}", $"{dadosPedido.cliente.nome} {dadosPedido.cliente.sobrenome}");
            arquivoWord.Range.Replace("{{cpf}}", dadosPedido.cliente.cpf.ToString(@"000\.000\.000\-00"));
            arquivoWord.Range.Replace("{{endereco}}", $"{dadosPedido.cliente.logradouro}, {dadosPedido.cliente.numEnd} {dadosPedido.cliente.compEnd ?? ""}");
            arquivoWord.Range.Replace("{{bairro}}", dadosPedido.cliente.bairro);
            arquivoWord.Range.Replace("{{cidade}}", dadosPedido.cliente.cidade);
            arquivoWord.Range.Replace("{{uf}}", dadosPedido.cliente.uf);
            arquivoWord.Range.Replace("{{cep}}", dadosPedido.cliente.cep);
            arquivoWord.Range.Replace("{{telefone}}", dadosPedido.cliente.telefone);

            arquivoWord.Range.Replace("{{Numero}}", dadosPedido.NF.ToString());
            arquivoWord.Range.Replace("{{dataHoraEmissao}}", dadosPedido.data.ToString("dd/MM/yyyy HH:mm"));
            arquivoWord.Range.Replace("{{valorTotal}}", dadosPedido.valorTotal.ToString("C2"));
            arquivoWord.Range.Replace("{{HoraEmissao}}", dadosPedido.data.ToString("HH:mm"));
            arquivoWord.Range.Replace("{{dataEmissao}}", dadosPedido.data.ToString("dd/MM/yyyy"));

            if (dadosPedido.itensPedido.Count() == 1)
            {
                foreach (var item in dadosPedido.itensPedido)
                {
                    arquivoWord.Range.Replace("{{codProd}}", item.produto.codProd.ToString());
                    arquivoWord.Range.Replace("{{nomeProd}}", item.produto.nomeProd.ToString());
                    arquivoWord.Range.Replace("{{qtdItem}}", item.qtdItem.ToString());
                    arquivoWord.Range.Replace("{{valorItem}}", item.valorItem.ToString("C2"));
                }
            }
            else
            {
                // Encontre a tabela que contém o marcador {{codProd}}
                Table tabela = arquivoWord.GetChildNodes(NodeType.Table, true)
                                          .Cast<Table>()
                                          .FirstOrDefault(t => t.Range.Text.Contains("{{codProd}}"));

                if (tabela != null)
                {
                    // Encontre a linha base com o marcador {{codProd}}
                    Row linhaBase = tabela.Rows.Cast<Row>()
                                               .FirstOrDefault(r => r.Range.Text.Contains("{{codProd}}"));

                    if (linhaBase != null)
                    {
                        // Clonando a linha base antes de removê-la
                        Row linhaModelo = (Row)linhaBase.Clone(true);

                        // Formantando o valor do Item do e jeito correto
                        string valor = dadosPedido.itensPedido.First().valorItem.ToString();
                        string valorItem = valor.Insert(valor.Length - 2, ",");

                        // Alterando a linha que já está no documento
                        arquivoWord.Range.Replace("{{codProd}}", dadosPedido.itensPedido.First().produto.codProd.ToString());
                        arquivoWord.Range.Replace("{{nomeProd}}", dadosPedido.itensPedido.First().produto.nomeProd.ToString());
                        arquivoWord.Range.Replace("{{qtdItem}}", dadosPedido.itensPedido.First().qtdItem.ToString());
                        arquivoWord.Range.Replace("{{valorItem}}", valorItem);

                        // Adicionar uma nova linha para cada item do pedido
                        foreach (var item in dadosPedido.itensPedido.Skip(1))
                        {
                            Row novaLinha = (Row)linhaModelo.Clone(true);

                            tabela.Rows.Add(novaLinha);

                            valor = item.valorItem.ToString();
                            valorItem = valor.Insert(valor.Length - 2, ",");

                            arquivoWord.Range.Replace("{{codProd}}", item.produto.codProd.ToString());
                            arquivoWord.Range.Replace("{{nomeProd}}", item.produto.nomeProd.ToString());
                            arquivoWord.Range.Replace("{{qtdItem}}", item.qtdItem.ToString());
                            arquivoWord.Range.Replace("{{valorItem}}", valorItem);
                        }
                    }
                }
            }

            // Adicionando o número de páginas
            arquivoWord.Range.Replace("{{qtd}}", arquivoWord.PageCount.ToString());

            // Criando um novo arquivo na memória para esse word em pdf
            MemoryStream arquivoPdf = new MemoryStream();

            // Salva o arquivo em pdf (converte)
            arquivoWord.Save(arquivoPdf, SaveFormat.Pdf);

            // Garante que o arquivo em pdf seja o primeiro na memória
            arquivoPdf.Position = 0;

            // Retorna o arquivo em pdf para visualização
            return File(arquivoPdf.ToArray(), "application/pdf", "documento-editado.pdf");
        }



        [HttpPost]
        public IActionResult AtualizarStatusPedido(int NF, int status)
        {
            // Pega o codLogin do Usuário Logado através da sessão
            var Login = _loginSessao.GetLogin();

            _pedidoRepositorio.AtualizarStatusPedido(NF, status);

            Pedido pedidoAtulizado = _pedidoRepositorio.ConsultarDetalhesPedido(NF);

            // Adiciona os dados do cliente ao pedido
            pedidoAtulizado.cliente = _clienteRepositorio.ConsultarCliente(Login.codLogin);

            return View("DetalhesPedido", pedidoAtulizado);
        }
    }
}

