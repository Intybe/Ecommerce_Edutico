namespace edutico.Models
{
    public class Pedido
    {
        public int NF { get; set; }
        public DateTime data { get; set; }
        public Cliente cliente { get; set; }
        public int statusPedido { get; set; }
        public decimal valorTotal { get; set; }
        public List<ItemPedido> itensPedido { get; set; }
    }
}
