namespace edutico.Models
{
    public class Imagem
    {
        public string nomeImg { get; set; }
        public string enderecoImg { get; set; }
        public decimal codProd { get; set; }
        public Produto produto { get; set; }
    }
}
