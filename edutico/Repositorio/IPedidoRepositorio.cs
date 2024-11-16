using edutico.Models;

namespace edutico.Repositorio
{
    public interface IPedidoRepositorio
    {
        string CadastrarPedido(Pedido pedido);

        List<Pedido> ConsultarPedidos(int codLogin);

        List<Pedido> ConsultarPedidosFiltros(int codLogin, int statusPedido);

        Pedido ConsultarDetalhesPedido(int NF);
    }
}
