namespace edutico.Models
{
    public class ItemPedido
    {
        public int codItem { get; set; }
        public Produto produto { get; set; }
        public int qtdItem { get; set; }
        public decimal valorItem { get; set; }
    }
}
