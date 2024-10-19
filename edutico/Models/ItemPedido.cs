namespace edutico.Models
{
    public class ItemPedido
    {
        public Produto produto { get; set; }
        public int qtdItem { get; set; }
        public decimal valorItem { get; set; }
    }
}
