using edutico.Models;

namespace edutico.Repositorio
{
    public interface IPedidoRepositorio
    {
        // Método para Cadastrar o pedido
        string CadastrarPedido(Pedido pedido);

        // Método para consultar os pedidos do cliente
        List<Pedido> ConsultarPedidos(int codLogin);

        // Método para consultar todos pedidos (Funcionário)
        List<Pedido> ConsultarPedidos();

        // Método para consultar pedidos por filtro
        List<Pedido> ConsultarPedidosFiltros(int codLogin, int statusPedido);

        // Método para consultar os detalhes do pedido
        Pedido ConsultarDetalhesPedido(int NF);

        // Método para Atulizar status do pedido
        void AtualizarStatusPedido(int NF, int status);
    }
}
