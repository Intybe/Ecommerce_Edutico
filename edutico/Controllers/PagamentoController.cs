using Microsoft.AspNetCore.Mvc;
using edutico.Models;
using edutico.Libraries.Login;
using edutico.Repositorio;
using Newtonsoft.Json;

namespace edutico.Controllers
{
    public class PagamentoController : Controller
    {
        private IClienteRepositorio? _clienteRepositorio;
        private ICartaoRepositorio? _cartaoRepositorio;
        private IPagamentoRepositorio? _pagamentoRepositorio;
        private readonly LoginSessao _loginSessao;

        public PagamentoController(IClienteRepositorio clienteRepositorio, LoginSessao loginSessao, ICartaoRepositorio cartaoRepositorio, IPagamentoRepositorio pagamentoRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _loginSessao = loginSessao; // Inicializa a variável _loginSessao
            _cartaoRepositorio = cartaoRepositorio;
            _pagamentoRepositorio = pagamentoRepositorio;
        }

        public IActionResult PagamentoQrcode()
        {
            Pagamento pagamento = null;

            var pagamentoJson = TempData["Pagamento"] as string;
            if (pagamentoJson != null)
            {
                pagamento = JsonConvert.DeserializeObject<Pagamento>(pagamentoJson);
            }

            if (pagamento.retorno == true)
            {
                _pagamentoRepositorio.PagamentoPix(pagamento);
            }

            return View(pagamento);
        }
        public IActionResult VisualizarCartao(string cartaoEnviado)
        {
            // Desserializa o JSON recebido de volta para um objeto Cartao
            var cartao = JsonConvert.DeserializeObject<Cartao>(cartaoEnviado);

            return View(cartao);
        }

        public IActionResult PagamentoCartao()
        {
            var Login = _loginSessao.GetLogin();

            Pagamento pagamento = null;

            var pagamentoJson = TempData["Pagamento"] as string;
            if (pagamentoJson != null)
            {
                pagamento = JsonConvert.DeserializeObject<Pagamento>(pagamentoJson);

            }

            pagamento.cartoes = _cartaoRepositorio.ConsultarCartao(Login.codLogin);

            // Passa a lista de cartões para a view
            return View(pagamento);
        }

        [HttpPost]
        public IActionResult CadastrarPagamentoCartao(int qtdParcela, string dadosPagamento, string dadosCartao)
        {
            // Desserializa o JSON recebido de volta para um objeto Pagamento
            var pagamento = JsonConvert.DeserializeObject<Pagamento>(dadosPagamento);

            // Desserializa o JSON recebido de volta para um objeto Cartao
            var cartao = JsonConvert.DeserializeObject<Cartao>(dadosCartao);

            string mensagem = _pagamentoRepositorio.PagamentoCartao(pagamento.pedido.NF, qtdParcela, cartao.codCartao);

            pagamento.mensagem = mensagem;

            // Armazena o objeto pagamento em TempData
            TempData["Pagamento"] = JsonConvert.SerializeObject(pagamento);

            return RedirectToAction("PagamentoCartao");
        }
    }
}