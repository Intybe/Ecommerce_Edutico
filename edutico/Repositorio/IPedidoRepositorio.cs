using edutico.Models;
using Edutico.Models;

namespace edutico.Repositorio
{
    public interface IPedidoRepositorio
    {
        string CadastrarPedido(Pedido pedido, List<ItemPedido> itensPedido);
    }
}
