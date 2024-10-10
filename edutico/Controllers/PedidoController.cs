using edutico.Libraries.Login;
using edutico.Repositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using edutico.Models;
using Edutico.Models;

namespace edutico.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly LoginSessao _loginSessao;

        public PedidoController(IPedidoRepositorio pedidoRepositorio, LoginSessao loginSessao)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _loginSessao = loginSessao;
        }

        // Método para exibir a página de finalizar o pedido
        [HttpGet]
        public IActionResult FinalizarPedido()
        {
            return View();
        }

        // Método POST para cadastrar o pedido e os itens
        [HttpPost]
        public IActionResult CadastrarPedido(Pedido pedido, List<ItemPedido> itensPedido)
        {
            if (ModelState.IsValid)
            {
                // Chama o método do repositório para cadastrar o pedido e seus itens
                string resultado = _pedidoRepositorio.CadastrarPedido(pedido, itensPedido);

                // Verifica se o pedido foi cadastrado com sucesso
                if (resultado == "Pedido cadastrado com sucesso!")
                {
                    // Redireciona automaticamente para a página de pagamento padrão (ex: cartão de crédito)
                    return RedirectToAction("PagamentoCartao", "Pagamento");
                }
                else
                {
                    // Em caso de erro, exibe a mensagem de erro na própria página
                    ViewBag.Erro = resultado;
                    return View("FinalizarPedido");
                }
            }

            // Se o ModelState não for válido, retorna à página de finalização de pedido
            return View("FinalizarPedido");
        }
    }
}

